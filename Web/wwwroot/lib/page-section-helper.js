var DemoPageSectionHelper = (function() {
    function initCopyCodeButtons(id) {
        var sectionSelector = id ? '#section-' + id + ' ' : '';
        var codeAreaEl = document.querySelector(sectionSelector + '.demo-page-section-code-area');
        var copyCodeBtn = codeAreaEl && codeAreaEl.querySelector('.btn.copy-code');
        if(!copyCodeBtn) return;
        
        new ClipboardJS(copyCodeBtn, {
            text: function () {
                var codeContainerEl = codeAreaEl.querySelector('.code-container');
                var codeEl = codeAreaEl.querySelector('pre > code');
                return codeEl && codeEl.textContent;
            }
        });
    }
    function initExpandCodeButtons(element) {
        var expandBtns = element.querySelectorAll('.more-code-btn');
        for(var i = 0; i < expandBtns.length; i++) {
            (function (btn) { btn.addEventListener("click", function () { expandCode(btn); }) })(expandBtns[i]);
        }
    }
    function expandCode(element) {
        element.parentNode.outerHTML = element.nextSibling.innerHTML;
    }

    return {
        init: function (id) {
            initCopyCodeButtons(id);
        },
        initExpandCodeButtons: initExpandCodeButtons
    };
})();
