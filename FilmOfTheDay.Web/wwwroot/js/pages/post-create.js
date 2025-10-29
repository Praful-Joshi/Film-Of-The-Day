document.addEventListener('DOMContentLoaded', () => {
    let selectedCard = null;
    let expanded = false;

    const movieCards = document.querySelectorAll('.movie-card');
    const toggleBtn = document.getElementById('toggleBtn');

    const inputTitle = document.getElementById('SelectedMovieTitle');
    const inputPoster = document.getElementById('SelectedMoviePosterUrl');
    const inputLink = document.getElementById('SelectedMovieLink');

    function selectCard(card) {
        if (selectedCard) {
            selectedCard.querySelector('.movie-select-card').classList.remove('bg-green-400');
        }
        selectedCard = card.closest('.movie-card');
        selectedCard.querySelector('.movie-select-card').classList.add('bg-green-400');

        inputTitle.value = card.dataset.title || '';
        inputPoster.value = card.dataset.poster || '';
        inputLink.value = card.dataset.link || '';
    }

    function getVisibleCount() {
        return window.innerWidth < 768 ? 4 : 12;
    }

    function updateVisibleMovies() {
        const visibleCount = getVisibleCount();
        movieCards.forEach((card, index) => {
            card.classList.toggle('hidden', !expanded && index >= visibleCount);
        });
        if (toggleBtn)
            toggleBtn.textContent = expanded ? 'Show less' : 'Show more';
    }

    // Initialize
    document.querySelectorAll('.movie-select-card').forEach(card => {
        card.addEventListener('click', () => selectCard(card));
    });

    updateVisibleMovies();

    window.addEventListener('resize', () => { if (!expanded) updateVisibleMovies(); });
    toggleBtn?.addEventListener('click', () => { expanded = !expanded; updateVisibleMovies(); });
});
