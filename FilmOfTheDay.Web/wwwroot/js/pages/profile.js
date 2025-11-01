// wwwroot/js/pages/profile.js

document.addEventListener('DOMContentLoaded', () => {
    // =========================
    // TAB SWITCHING LOGIC
    // =========================
    function showTab(tabName) {
        const tabButtons = document.querySelectorAll('.tab-btn');
        const tabContents = document.querySelectorAll('.tab-content');

        // Hide all tab contents
        tabContents.forEach(tab => tab.classList.add('hidden'));

        // Update tab button colors
        tabButtons.forEach(btn => {
            btn.classList.remove('text-brand-text-light');
            btn.classList.add('text-brand-text-muted');
        });

        // Activate clicked tab
        const activeButton = Array.from(tabButtons).find(btn =>
            btn.getAttribute('onclick')?.includes(tabName)
        );
        if (activeButton) {
            activeButton.classList.remove('text-brand-text-muted');
            activeButton.classList.add('text-brand-text-light');
        }

        // Show the selected tab
        const activeTab = document.getElementById(tabName);
        if (activeTab) activeTab.classList.remove('hidden');
    }


    // expose showTab globally for inline onclick() bindings
    window.showTab = showTab;

    // =========================
    // FOLLOW / FRIEND LOGIC
    // =========================
    const followForm = document.querySelector('.follow-form');
    const followBtn = document.getElementById('followBtn');

    if (!followForm || !followBtn) return;

    // Case 1: "Accept Request" button (type="button")
    if (followBtn.type === 'button') {
        followBtn.addEventListener('click', async () => {
            const senderId = followBtn.getAttribute('data-receiver');
            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

            followBtn.disabled = true;
            followBtn.textContent = 'Accepting...';

            try {
                const res = await fetch('/Connection/AcceptRequest', {
                    method: 'POST',
                    credentials: 'same-origin',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': token || ''
                    },
                    body: JSON.stringify(parseInt(senderId))
                });

                if (res.ok) {
                    followBtn.textContent = 'Friends';
                } else {
                    followBtn.textContent = 'Error';
                    followBtn.disabled = false;
                }
            } catch (err) {
                console.error('Accept request failed:', err);
                followBtn.textContent = 'Try Again';
                followBtn.disabled = false;
            }
        });
    }

    // Case 2: Regular "Send Request" form submit
    followForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        followBtn.disabled = true;
        followBtn.textContent = 'Sending...';

        const formData = new FormData(followForm);

        try {
            const url = followForm.action?.length > 0 ? followForm.action : '/Connection/SendRequest';

            const response = await fetch(url, {
                method: 'POST',
                body: formData,
                credentials: 'same-origin',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            if (response.ok) {
                followBtn.textContent = 'Requested';
            } else {
                followBtn.textContent = 'Error';
                followBtn.disabled = false;
            }
        } catch (err) {
            console.error('Follow request failed:', err);
            followBtn.textContent = 'Try Again';
            followBtn.disabled = false;
        }
    });
});
