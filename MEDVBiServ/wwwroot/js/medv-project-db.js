window.medvProjectDb = (function () {
    const DB = "medv_projects";
    const VERSION = 1;
    const STORE = "projects";

    function open() {
        return new Promise((resolve, reject) => {
            const req = indexedDB.open(DB, VERSION);

            req.onupgradeneeded = () => {
                const db = req.result;
                if (!db.objectStoreNames.contains(STORE)) {
                    const store = db.createObjectStore(STORE, { keyPath: "id" });
                    store.createIndex("createdAt", "createdAt");
                    store.createIndex("title", "title");
                }
            };

            req.onsuccess = () => resolve(req.result);
            req.onerror = () => reject(req.error);
        });
    }

    async function save(project) {
        const db = await open();
        return new Promise((res, rej) => {
            const tx = db.transaction(STORE, "readwrite");
            tx.objectStore(STORE).put(project);
            tx.oncomplete = () => res(true);
            tx.onerror = () => rej(tx.error);
        });
    }

    async function list() {
        const db = await open();
        return new Promise((res, rej) => {
            const tx = db.transaction(STORE, "readonly");
            const req = tx.objectStore(STORE).getAll();
            req.onsuccess = () => res(req.result || []);
            req.onerror = () => rej(req.error);
        });
    }

    async function get(id) {
        const db = await open();
        return new Promise((res, rej) => {
            const tx = db.transaction(STORE, "readonly");
            const req = tx.objectStore(STORE).get(id);
            req.onsuccess = () => res(req.result || null);
            req.onerror = () => rej(req.error);
        });
    }

    async function remove(id) {
        const db = await open();
        return new Promise((res, rej) => {
            const tx = db.transaction(STORE, "readwrite");
            tx.objectStore(STORE).delete(id);
            tx.oncomplete = () => res(true);
            tx.onerror = () => rej(tx.error);
        });
    }

    return { save, list, get, remove };
})();
