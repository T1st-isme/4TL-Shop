function ChangeImage(uploadimg, previewImg) {
    if (uploadimg.files && uploadimg.files[0]) {
        var render = new FileReader();
        render.onload = function (e) {
            $(previewImg).attr('src', e.target.result);
        }
        render.readAsDataURL(uploadimg.files[0]);
    }
}
