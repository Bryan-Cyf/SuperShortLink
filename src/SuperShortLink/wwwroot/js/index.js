function generate() {
    var url = $("#inp-url").val();
    $.post("/api/ShortLink/Generate", { url: url }, function (result) {
        if (!result || !!result.msg) {
            alert(result.msg);
            return;
        }
        $("#newUrl").html(window.location.origin + "/" + result).prop("href", window.location.origin + "/" + result);
        $("#oldUrl").html(url).prop("href", url);
        $("#codeImg").prop("src", "/api/ShortLink/GetQRCodeImage?url=" + $("#newUrl").html());
    });
}