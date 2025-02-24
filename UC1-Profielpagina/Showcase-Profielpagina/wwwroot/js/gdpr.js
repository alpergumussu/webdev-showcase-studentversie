class GDPR {
    constructor() {
        let gdprElement = document.querySelector('.gdpr-consent');

        if (gdprElement) {
            gdprElement.style.display = 'none';
        }

        if (this.cookieStatus() !== 'accept' && this.cookieStatus() !== 'reject') {
            this.showGDPR();
        }

        this.showStatus();
        this.showContent();
        this.bindEvents();
    }

    bindEvents() {
        let buttonAccept = document.querySelector('.gdpr-consent__button--accept');
        let buttonReject = document.querySelector('.gdpr-consent__button--reject');

        buttonAccept.addEventListener('click', () => {
            this.cookieStatus('accept');
            this.showStatus();
            this.showContent();
            this.hideGDPR();
        });

        buttonReject.addEventListener('click', () => {
            this.cookieStatus('reject');
            this.showStatus();
            this.showContent();
            this.hideGDPR();
        });
    }

    showContent() {
        this.resetContent();
        const status = this.cookieStatus() == null ? 'not-chosen' : this.cookieStatus();
        const element = document.querySelector(`.content-gdpr-${status}`);
        if (element) {
            element.classList.add('show');
            element.classList.remove('hide');
        }
    }

    resetContent() {
        const classes = [
            '.content-gdpr-accept',
            '.content-gdpr-reject',
            '.content-gdpr-not-chosen'
        ];

        for (const c of classes) {
            let element = document.querySelector(c);
            if (element) {
                element.classList.add('hide');
                element.classList.remove('show');
            }
        }
    }

    showStatus() {
        let statusElement = document.getElementById('content-gpdr-consent-status');
        if (statusElement) {
            statusElement.innerHTML = this.cookieStatus() == null ? 'Niet gekozen' : this.cookieStatus();
        }
    }

    cookieStatus(status) {
        if (status) localStorage.setItem('gdpr-consent-choice', status);
        return localStorage.getItem('gdpr-consent-choice');
    }

    hideGDPR() {
        let gdprElement = document.querySelector('.gdpr-consent');
        if (gdprElement) {
            gdprElement.style.opacity = '0';
            gdprElement.style.transform = 'translateY(20px)';
            setTimeout(() => {
                gdprElement.style.display = 'none';
            }, 500); // Matches fade-out duration
        }
    }

    showGDPR() {
        let gdprElement = document.querySelector('.gdpr-consent');
        if (gdprElement) {
            gdprElement.style.display = 'block';
            setTimeout(() => {
                gdprElement.style.opacity = '1';
                gdprElement.style.transform = 'translateY(0)';
            }, 50); // Small delay to trigger transition
        }
    }
}

