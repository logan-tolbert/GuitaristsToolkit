document.addEventListener("DOMContentLoaded", function () {
    let controller = new AbortController();

   
    const deleteModal = document.getElementById("deleteModal");

    if (deleteModal) {
        document.body.addEventListener("click", function (event) {
            
            if (event.target.matches("[data-session-id]")) {
                const sessionId = event.target.getAttribute("data-session-id");
                const deleteUrl = `/Session/Delete/${sessionId}`;
                document.getElementById("confirmDeleteBtn").setAttribute("href", deleteUrl);
            }
        });
    }

    
    window.addEventListener("beforeunload", function () {
        controller.abort();
    });
});
