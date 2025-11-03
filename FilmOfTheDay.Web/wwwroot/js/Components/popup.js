document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('[data-popup-target]').forEach(trigger => {
        const popup = document.getElementById(trigger.dataset.popupTarget);
        if (!popup) return;

        const card = popup.querySelector('div');
        const cancelBtn = popup.querySelector('.cancel-popup');
        const confirmBtn = popup.querySelector('.confirm-popup');

        const open = () => {
            popup.classList.remove('hidden');
            setTimeout(() => {
                card.classList.remove('opacity-0', 'scale-95');
                card.classList.add('opacity-100', 'scale-100');
            }, 10);
        };
        const close = () => {
            card.classList.add('opacity-0', 'scale-95');
            setTimeout(() => popup.classList.add('hidden'), 200);
        };

        trigger.addEventListener('click', open);
        cancelBtn?.addEventListener('click', close);
        popup.addEventListener('click', e => { if (e.target === popup) close(); });

        confirmBtn?.addEventListener('click', async () => {
            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
            confirmBtn.disabled = true;
            confirmBtn.textContent = 'Removing...';

            try {
                const res = await fetch(confirmBtn.dataset.endpoint, {
                    method: 'POST',
                    credentials: 'same-origin',
                    headers: {
                        'X-Requested-With': 'XMLHttpRequest',
                        'RequestVerificationToken': token || ''
                    }
                });

                const data = await res.json().catch(() => ({})); // parse safely
                if (res.ok && data.success) {
                    // animate fade-out
                    setTimeout(() => {
                        card.classList.add('opacity-0', 'scale-95');
                        setTimeout(() => {
                            popup.classList.add('hidden');
                            // optional redirect
                            const redirectUrl = confirmBtn.dataset.redirect;
                            if (redirectUrl) window.location.href = redirectUrl;
                        }, 200);
                    }, 800);
                } else {
                    confirmBtn.textContent = 'Error';
                    confirmBtn.disabled = false;
                }
            } catch (err) {
                console.error('Popup remove failed:', err);
                confirmBtn.textContent = 'Try Again';
                confirmBtn.disabled = false;
            }
        });


    });
});
