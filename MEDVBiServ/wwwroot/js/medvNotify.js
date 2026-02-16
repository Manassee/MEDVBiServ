window.medvNotify = {
    init: () => {
        // optional – aktuell nichts nötig
    },

    requestPermission: async () => {
        if (!("Notification" in window)) return false;
        const res = await Notification.requestPermission();
        return res === "granted";
    },

    notify: (title, body) => {
        if (!("Notification" in window)) return;
        if (Notification.permission !== "granted") return;

        try {
            new Notification(title, { body });
        } catch {
            // ignore
        }
    }
};