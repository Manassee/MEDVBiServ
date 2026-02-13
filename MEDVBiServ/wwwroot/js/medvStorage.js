window.medvStorage = (function () {
    let registered = false;

    function register(dotNetRef) {
        if (registered) return;
        registered = true;

        window.addEventListener("storage", function (e) {
            // nur reagieren, wenn unsere Keys sich ändern
            if (!e) return;
            if (e.key === "slides" || e.key === "slidesCreatedAt") {
                dotNetRef.invokeMethodAsync("OnStorageChanged");
            }
        });
    }

    return { register };
})();