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
        
        var logoInput = new window.KTImageInput("quest_image");

        

        $(".chancelCrop").click(function () {

            window.KTUtil.removeClass(logoInput.element, 'image-input-changed');
            window.KTUtil.removeClass(logoInput.element, 'image-input-empty');
            window.KTUtil.css(logoInput.wrapper, 'background-image', logoInput.src);
            logoInput.input.value = "";

            if (logoInput.hidden) {
                logoInput.hidden.value = "0";
            }
            helper.invokeMethodAsync("InvokeMethod");
        });

        
    }
    
    return{
        init: function (helper) {
            run(helper);
        }
    }
}();
