document.addEventListener("DOMContentLoaded", function () {
    const deleteModal = document.getElementById("deleteModal");

    if (deleteModal) {
        document.body.addEventListener("click", function (event) {
            if (event.target.matches("[data-setlist-id]")) {
                const setlistId = event.target.getAttribute("data-setlist-id");
                const setlistName = event.target.getAttribute("data-setlist-name");

                document.getElementById("setlistName").textContent = setlistName;

                const deleteUrl = `/Setlist/Delete/${setlistId}`;
                document.getElementById("confirmDeleteBtn").setAttribute("href", deleteUrl);
            }
        });
    }
});
