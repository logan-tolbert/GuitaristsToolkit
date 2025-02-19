
    document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll(".delete-btn");

    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            const songId = this.getAttribute("data-song-id");
            const setlistId = this.getAttribute("data-setlist-id");

            document.getElementById("songIdInput").value = songId;
            document.getElementById("setlistIdInput").value = setlistId;

            // Set the form action dynamically
            document.getElementById("deleteForm").action = `/Song/Delete`;
        });
    });
});
