window.medvFs = (function () {

    function isSupported() {
        return !!window.showDirectoryPicker;
    }

    async function ensurePermission(dirHandle) {
        // Prüfen, ob wir schon Permission haben
        const p = await dirHandle.queryPermission({ mode: "readwrite" });
        if (p === "granted") return true;

        // Anfordern
        const r = await dirHandle.requestPermission({ mode: "readwrite" });
        return r === "granted";
    }

    async function pickDirectoryIfNeeded() {
        if (!isSupported()) return null;

        // 1) Versuch: vorhandenen Handle laden
        let dirHandle = await window.medvHandleStore.loadDirectoryHandle();

        // 2) Falls keiner da → Picker öffnen (nur beim ersten Export)
        if (!dirHandle) {
            dirHandle = await window.showDirectoryPicker({ mode: "readwrite" });
            const ok = await ensurePermission(dirHandle);
            if (!ok) throw new Error("Ordner-Berechtigung verweigert.");

            await window.medvHandleStore.saveDirectoryHandle(dirHandle);
            return dirHandle;
        }

        // 3) Falls Handle da → Permission sicherstellen
        const ok = await ensurePermission(dirHandle);
        if (!ok) {
            // Permission verloren → Picker erneut
            dirHandle = await window.showDirectoryPicker({ mode: "readwrite" });
            const ok2 = await ensurePermission(dirHandle);
            if (!ok2) throw new Error("Ordner-Berechtigung verweigert.");

            await window.medvHandleStore.saveDirectoryHandle(dirHandle);
            return dirHandle;
        }

        return dirHandle;
    }

    async function saveFileToAppFolder(fileName, base64) {
        const dir = await pickDirectoryIfNeeded();
        if (!dir) throw new Error("File System API nicht verfügbar.");

        const fileHandle = await dir.getFileHandle(fileName, { create: true });
        const writable = await fileHandle.createWritable();

        const bytes = Uint8Array.from(atob(base64), c => c.charCodeAt(0));
        await writable.write(bytes);
        await writable.close();

        return true;
    }

    return {
        isSupported,
        saveFileToAppFolder
    };
})();
