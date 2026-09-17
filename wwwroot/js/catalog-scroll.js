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
    saveProducts: function (key, dataJson) {
        try {
            localStorage.setItem(key, dataJson);
            return true;
        } catch (e) {
            console.error('Error saving to localStorage', e);
            return false;
        }
    },
    getProducts: function (key) {
        try {
            return localStorage.getItem(key);
        } catch (e) {
            console.error('Error reading from localStorage', e);
            return null;
        }
    },
    isOnline: function () {
        return navigator.onLine;
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
