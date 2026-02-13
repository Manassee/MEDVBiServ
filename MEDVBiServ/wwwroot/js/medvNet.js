window.medvNet = (function () {
    let registered = false;

    function isOnline() {
        return navigator.onLine === true;
    }

    function register(dotNetRef) {
        if (registered) return;
        registered = true;

        const notify = () => dotNetRef.invokeMethodAsync("OnNetChanged", isOnline());

        window.addEventListener("online", notify);
        window.addEventListener("offline", notify);

        // initial push
        notify();
    }

    return { isOnline, register };
})();