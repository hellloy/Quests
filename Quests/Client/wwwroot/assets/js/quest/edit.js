"use strict";
// Class definition

var KTSummernote = function () {
    // Private functions
    var run = function (helper) {
        console.log('init summernote');
        $('.summernote').summernote({
            height: 400,
            tabsize: 2,
            callbacks: {
                onChange: function (contents, $editable) {
                    helper.invokeMethodAsync("GetSummerNoteValue", contents);
                }
            }
        });
        $('.summernote').summernote('code', $('.summernote').val());
    }

    return {
        // public functions
        init: function (helper) {
            run(helper);
        }
    };
}();

var ImageInput = function () {
    var run = function (helper) {
        console.log('init image input');
        var logoInput = new window.KTImageInput("quest_image");

        var $image = $('#image');
        var cropBoxData;
        var canvasData;


        $('#cropperModal').on('shown.bs.modal',
            function () {
                $image.cropper({
                    autoCropArea: 1,
                    dragMode: "move",
                    aspectRatio: 16 / 9,
                    ready: function () {
                        $image.cropper('setCanvasData', canvasData);
                        $image.cropper('setCropBoxData', cropBoxData);
                    }
                });

            }).on('hidden.bs.modal',
                function () {
                    $image.cropper('destroy');
                    console.log('Cropper Destroy');
                });


        logoInput.on('change',
            function (imageInput) {
                var input = imageInput.input;
                if (input.files && input.files[0]) {

                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $image.attr('src', e.target.result);
                    }
                    reader.readAsDataURL(input.files[0]);

                    $('#cropperModal').modal('show');
                }
            });


        $('#cropImage').on('click',
            function () {
                console.log('Start Ok');
                var croppedImage = $image.cropper('getCroppedCanvas');
                var canvasUrl = croppedImage.toDataURL("image/png");

                logoInput.wrapper.style.backgroundImage = `url(${canvasUrl})`;
                helper.invokeMethodAsync("InvokeMethod", canvasUrl);
                $("#cropperModal").modal("hide");
                console.log('End Ok');
            });

        $(".chancelCrop").click(function () {

            window.KTUtil.removeClass(logoInput.element, 'image-input-changed');
            window.KTUtil.removeClass(logoInput.element, 'image-input-empty');
            window.KTUtil.css(logoInput.wrapper, 'background-image', logoInput.src);
            logoInput.input.value = "";

            if (logoInput.hidden) {
                logoInput.hidden.value = "0";
            }
            helper.invokeMethodAsync("InvokeMethod", null);
        });
    }
    return{
        init: function (helper) {
            run(helper);
        }
    }
}();
