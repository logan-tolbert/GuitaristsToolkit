document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll(".delete-btn");

    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            const setlistId = this.getAttribute("data-setlist-id");
            const setlistName = this.getAttribute("data-setlist-name");

            document.getElementById("setlistName").textContent = setlistName;
            document.getElementById("deleteSetlistId").value = setlistId;

            // Ensure form action is correct
            document.getElementById("deleteForm").action = `/Setlist/Delete`;
        });
    });
});
