window.landingPage = (function () {
    let wordTimer = null;
    let observer = null;

    function initWordRotate() {
        const el = document.querySelector('.landing-word-rotate');
        if (!el) return;

        if (wordTimer) {
            clearInterval(wordTimer);
            wordTimer = null;
        }

        const words = (el.dataset.words || '').split(',').map(w => w.trim()).filter(Boolean);
        if (words.length < 2) return;

        let i = 0;
        wordTimer = setInterval(() => {
            i = (i + 1) % words.length;
            el.style.opacity = 0;
            el.style.transform = 'translateY(6px)';
            setTimeout(() => {
                el.textContent = words[i];
                el.style.opacity = 1;
                el.style.transform = 'translateY(0)';
            }, 220);
        }, 2200);
    }

    function initScrollReveal() {
        const items = document.querySelectorAll('.landing-reveal');
        if (!items.length) return;

        if (observer) {
            observer.disconnect();
        }

        observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('in-view');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.15 });

        items.forEach(item => observer.observe(item));
    }

    function initFloatingBadgeClose() {
        const btn = document.querySelector('.landing-floating-badge button');
        const badge = document.querySelector('.landing-floating-badge');
        if (btn && badge) {
            btn.onclick = () => { badge.style.display = 'none'; };
        }
    }

    function initMobileNavToggle() {
        const toggle = document.querySelector('.landing-nav-toggle');
        const links = document.querySelector('.landing-nav-links');
        if (toggle && links) {
            toggle.onclick = () => {
                const isOpen = links.style.display === 'flex';
                links.style.display = isOpen ? 'none' : 'flex';
                links.style.flexDirection = 'column';
                links.style.position = 'absolute';
                links.style.top = '64px';
                links.style.right = '16px';
                links.style.background = '#fff';
                links.style.padding = '16px 22px';
                links.style.borderRadius = '18px';
                links.style.boxShadow = '0 20px 40px rgba(17,17,17,0.15)';
            };
        }
    }

    function init() {
        initWordRotate();
        initScrollReveal();
        initFloatingBadgeClose();
        initMobileNavToggle();
    }

    return { init };
})();
