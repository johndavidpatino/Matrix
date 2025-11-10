export class NotificationManager {
    constructor(containerId, autoHideDelay = 5000) {
        this.container = document.getElementById(containerId);
        this.autoHideDelay = autoHideDelay;
    }

    showNotification(type, message) {
        const notification = document.createElement('div');
        notification.className = `notification ${type}`;
        notification.innerHTML = `
            <span class="notification-icon"></span>
            <p>${message}</p>
            <button class="close-btn">&times;</button>
        `;

        this.container.appendChild(notification);

        const closeBtn = notification.querySelector('.close-btn');

        let timeout = setTimeout(() => this.removeNotificationWithDelay(notification), this.autoHideDelay);

        closeBtn.addEventListener('click', (event) => {
            event.preventDefault()
            clearTimeout(timeout)
            this.removeNotification(notification)
        });

    }

    removeNotification(notification) {
        notification.classList.add('fade-out');
        setTimeout(() => {
            if (notification) {
                this.container.removeChild(notification);
            }
        }, 100);
    }

    removeNotificationWithDelay(notification, delay) {
        notification.classList.add('fade-out');
        setTimeout(() => {
            if (notification) {
                this.container.removeChild(notification);
            }
        }, delay);
    }

    showInfo(message) {
        this.showNotification('info', message);
    }

    showWarning(message) {
        this.showNotification('warning', message);
    }

    showSuccess(message) {
        this.showNotification('success', message);
    }

    showError(message) {
        this.showNotification('error', message);
    }
}
