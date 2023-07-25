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

    $("#message-form").on("submit", function (event) {
        event.preventDefault();
        successMessage.slideDown().delay(2000).slideUp();
        messageText.val("");
        messageCounter.text("Characters left: 3000");
        messageCounter.css("color", "#000");
    });

    $("#message-form").on("submit", function(event) {
        event.preventDefault();
        const message = $("#message-text").val();
        if (message.trim() === "") {
          alert("Please enter a message.");
          return;
        }
        const dataToSend = {
          username: username,
          message: message
        };
        $.ajax({
          url: "/Details",
          method: "POST",
          contentType: "application/json",
          data: JSON.stringify(dataToSend),
          success: function(response) {
            if (response.Success) {
              successMessage.css("background-color", "#4caf50");
            } else {
              successMessage.css("background-color", "red");
            }
            successMessage.text(response.Message);
            successMessage.slideDown().delay(2000).slideUp();
            messageText.val("");
            messageCounter.text("Characters left: 3000");
            messageCounter.css("color", "#000");
          },
          error: function(xhr, status, error) {
            successMessage.css("background-color", "red");
            successMessage.text("Error encountered while sending the message.");
            successMessage.slideDown().delay(2000).slideUp();
            messageText.val("");
            messageCounter.text("Characters left: 3000");
            messageCounter.css("color", "#000");
          }
        });
      });
});