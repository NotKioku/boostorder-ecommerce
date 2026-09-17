window.catalogScroll = {
    observer: null,
    scrollListener: null,
    init: function (dotNetHelper, elementId) {
        this.dispose();
        const sentinel = document.getElementById(elementId);
        if (!sentinel) {
            return false;
        }

        let isTriggering = false;
        const trigger = () => {
            if (!isTriggering) {
                isTriggering = true;
                dotNetHelper.invokeMethodAsync('OnScrollToBottom').finally(() => {
                    setTimeout(() => { isTriggering = false; }, 300);
                });
            }
        };

        if ('IntersectionObserver' in window) {
            this.observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        trigger();
                    }
                });
            }, {
                root: null,
                rootMargin: '300px',
                threshold: 0
            });
            this.observer.observe(sentinel);
        }

        this.scrollListener = () => {
            const rect = sentinel.getBoundingClientRect();
            if (rect.top <= window.innerHeight + 300) {
                trigger();
            }
        };
        window.addEventListener('scroll', this.scrollListener, { passive: true });

        return true;
    },
    dispose: function () {
        if (this.observer) {
            this.observer.disconnect();
            this.observer = null;
        }
        if (this.scrollListener) {
            window.removeEventListener('scroll', this.scrollListener);
            this.scrollListener = null;
        }
    }
};
