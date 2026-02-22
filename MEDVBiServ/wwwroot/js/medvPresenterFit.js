window.medvPresenterFit = (function () {

    function px(n) { return n + "px"; }

    function setSize(el, s) {
        if (!el) return;
        el.style.fontSize = px(s);
    }

    function fits(box) {
        return box.scrollHeight <= box.clientHeight;
    }

    function binaryFit(box, el, min, max) {
        if (!box || !el) return;

        setSize(el, max);

        if (fits(box)) return;

        let lo = min;
        let hi = max;

        while (hi - lo > 1) {
            const mid = Math.floor((lo + hi) / 2);
            setSize(el, mid);
            if (fits(box)) lo = mid;
            else hi = mid;
        }

        setSize(el, lo);
    }

    function fit(boxId, titleId, textId, opts) {
        const box = document.getElementById(boxId);
        const title = document.getElementById(titleId);
        const text = document.getElementById(textId);

        if (!box) return;

        const o = opts || {};
        const titleMin = o.titleMin ?? 40;
        const titleMax = o.titleMax ?? 130;
        const textMin = o.textMin ?? 50;
        const textMax = o.textMax ?? 180;

        if (text) binaryFit(box, text, textMin, textMax);
        if (title) binaryFit(box, title, titleMin, titleMax);
    }

    return { fit };

})();