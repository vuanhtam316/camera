var site = site || {};

site.config = {

},

site.product = {
    init: function () {


    }
}

site.productCreate = {
    productCreate: function () {
        $('#resetform').click(function () {
            $('#frmCreate').each(function () {
                this.reset();
            });
        });
        $(function () {
            $("#SelectImages").click(function () {
                var finder = new CKFinder();
                finder.selectActionFunction = function (fileUrl) {
                    $("#txtImageSP").val(fileUrl);
                };
                finder.popup();
            });
        });
        $(function () {
            createEditor('vi', 'Product_Contents');
        });
        function failure() {
            MessageErro('Có lỗi xảy ra trong quá trình xử lý. Xin vui lòng thử lại');
            $("#totop").click();
        }

        function success() {
            $("#totop").click();
        }
    }
};