window.medvPresenter = (function () {
    const CHANNEL = "medv-presenter-sync";
    let bc = null;
    let dotnet = null;
    let presenterDotnet = null;
    let screenDotnet = null;


    function ensureChannel() {
        if (bc) return bc;

        bc = new BroadcastChannel(CHANNEL);

        bc.onmessage = (ev) => {
            const msg = ev.data;
            if (!msg || !msg.type) return;

            // Presenter beantwortet State-Anfragen
            if (presenterDotnet && msg.type === "REQUEST_STATE") {
                presenterDotnet.invokeMethodAsync("OnChannelMessage", "REQUEST_STATE");
            }

            // Screen empfängt State Updates
            if (screenDotnet && msg.type === "STATE") {
                screenDotnet.invokeMethodAsync("OnState", JSON.stringify(msg.payload));
            }
        };

        return bc;
    }

    function registerDotNet(dotNetRef) {
        presenterDotnet = dotNetRef;
        ensureChannel();
    }

    function registerScreen(dotNetRef) {
        screenDotnet = dotNetRef;
        ensureChannel();
    }

    function sendState(state) {
        ensureChannel().postMessage({ type: "STATE", payload: state });
    }
    function requestState() {
        ensureChannel().postMessage({ type: "REQUEST_STATE" });
    }
    function toggleFullscreen() {
        const el = document.documentElement;
        if (!document.fullscreenElement) el.requestFullscreen?.();
        else document.exitFullscreen?.();
    }

    function openBeamer(url) {
        return window.open(url, "_blank");
    }

    return {
        registerDotNet,
        registerScreen,
        sendState,
        requestState,
        toggleFullscreen,
        openBeamer
    };
})();
