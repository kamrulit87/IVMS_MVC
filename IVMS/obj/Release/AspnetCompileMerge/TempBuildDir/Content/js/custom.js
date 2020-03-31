function openReportAssignFormat(reportFormat, data, rptTittle) {
    var sliceSize = 512;
    data = ("" + data).replace(/^[^,]+,/, ''); //Remove ^ from ParamCaption we use replace(/^[^,]+,/, '')
    data = ("" + data).replace(/\s/g, '');  //Remove space from ParamCaption we use replace(/[\s]/g, '')	
	
	//alert(data);
    var byteCharacters = window.atob(data);  //Bind with page by data 
	
	
    var byteArrays = [];
    //Convert to array
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);
        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }
    //Convert to blob and open file in different format
    if (reportFormat === "pdf") {
        //var blob = new Blob(byteArrays, { type: "application/pdf" }, "Test.pdf");
        var blob = new Blob(byteArrays, { type: "application/pdf" }, "");
        var fileURL = window.URL.createObjectURL(blob);
        window.open(fileURL, '_blank');
    }

    if (reportFormat === "WORD" || reportFormat === "word") {

        var blob = new Blob(byteArrays, {
            type: "application/msword"
        });
        var blobURL = (window.URL || window.webkitURL).createObjectURL(blob);
        var anchor = document.createElement("a");
        anchor.download = rptTittle + ".doc";;
        anchor.href = blobURL;
        anchor.click();
        window.open(blobURL, '_blank');
    }
    if (reportFormat === "Excel") {
        var blob = new Blob(byteArrays, { type: "application/vnd.ms-excel" });
        var blobURL = (window.URL || window.webkitURL).createObjectURL(blob);
        var anchor = document.createElement("a");
        anchor.download = rptTittle + ".xls";
        anchor.href = blobURL;
        anchor.click();
        window.open(blobURL, '_blank');
    }
}
 
function printDocument(_url, _transactionID, _confirmationMsg) {
    if (_confirmationMsg == '') {
        _confirmationMsg = 'Do you want to view this transaction ?';
    }
    if (confirm(_confirmationMsg)) {
        $.ajax({
            url: _url,
            type: "POST",
            data: { id: _transactionID },
            beforeSend: function () {
                $('#loading').show();
            },
            complete: function () {
                setTimeout(function () { $("#loading").hide(); }, 2000);
            },
            success: function (data) {
                var rptTittle = "Test";
                openReportAssignFormat('pdf', data, rptTittle);
            },
            error: function (xhr) {
                alert('error');
            }
        });
    }
}
//function printBaseOnMultiParameter(_url, _parameters) {
//    $.ajax({
//        url: _url,
//        type: "POST",
//        data: { parameters: _parameters },
//        success: function (data) {
//            var rptTittle = "Test";
//            openReportAssignFormat('pdf', data, rptTittle);
//        },
//        error: function (xhr) {
//            alert('error');
//        }
//    });
//}

function printBaseOnMultiParameter(_url, _parameters, _confirmationMsg) {
    
    if (_confirmationMsg == '') {
        _confirmationMsg = 'Do you want to view this transaction ?';
    }
    if (confirm(_confirmationMsg)) {
        $.ajax({
            url: _url,
            type: "POST",
            data: { parameters: _parameters },
            beforeSend: function () {
                $('#loading').show();
            },
            complete: function () {
                setTimeout(function () { $("#loading").hide(); }, 2000);
            },
            success: function (data) {
                var rptTittle = "Test";
                openReportAssignFormat('pdf', data, rptTittle);
            },
            error: function (xhr) {
                alert('error');
            }
        });
    }

}

//Added by kamrul 06.09.2016
$(".IsNumber").keydown(function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
        (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
});
