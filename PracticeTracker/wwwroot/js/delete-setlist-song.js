document.addEventListener("DOMContentLoaded", function () {
    const deleteModal = document.getElementById("deleteModal");

    if (deleteModal) {
        document.body.addEventListener("click", function (event) {
            if (event.target.matches("[data-song-id]")) {
                
                const songId = event.target.getAttribute("data-song-id");
                const setlistId = event.target.getAttribute("data-setlist-id");


                const deleteUrl = `/Setlist/DeleteSong?setlistId=${setlistId}&songId=${songId}`;
                document.getElementById("confirmDeleteBtn").setAttribute("href", deleteUrl);

            }
        });
    }
});
