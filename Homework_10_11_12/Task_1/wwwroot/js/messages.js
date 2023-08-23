$(document).ready(function () {
    const messageText = $("#message-text");
    const messageCounter = $("#message-counter");
    const sendButton = $("#send-button");
    const successMessage = $("#success-message");
    const username = $("#usernameSpan");

    messageText.on("input", function () {
        const length = this.value.length;
        const maxLength = parseInt(this.getAttribute("maxlength"));
        const remaining = maxLength - length;
        messageCounter.text(`Characters left: ${remaining}`);

        if (length >= maxLength) {
            messageCounter.css("color", "red");
            sendButton.attr("disabled", true);
        } else {
            messageCounter.css("color", "#000");
            sendButton.attr("disabled", false);
        }
    });
});