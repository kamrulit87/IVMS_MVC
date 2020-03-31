function vatCalculation(total,vatPercent,isLineEnabled){
    vatAmount = 0.00;
    if (!isNaN(total) && total > 0) {
        vatAmount = (vatPercent * total) / 100
    }
    //var isLineEnabled = @ViewBag.LineVatEnabled;
    if(isLineEnabled == true){
        $('#LineVat').val(vatAmount.toFixed(2));
    }else{
        $('#TotalVat').val(vatAmount.toFixed(2));
    }
}
// this id need for this function  #CDiscountPercent, #TotalVatPercent, #TotalDiscount, #TotalVat
function calculateTotalVatNDiscount() {
    var cDiscountPercent = $('#CDiscountPercent').val();
    var totalVatPercent = $('#TotalVatPercent').val();
    var totalDiscount = 0;
    if (cDiscountPercent > 0) {
        totalDiscount = (SubTotal * cDiscountPercent) / 100;
    } else {
        totalDiscount = Discount;
    }
    $("#TotalDiscount").val(totalDiscount);
    $("#TotalVat").val(VatAmount);
}

function calculateVatNDiscount(vatPercent,discountPercent,isVatLine,isDiscountLine) {    
    var discountAmt = 0;   
    if (discountPercent > 0) {
        discountAmt = (SubTotal * discountPercent) / 100;
    }

    if (isVatLine) {
        $("#LineVat").val(discountAmt);
    } else {
        $("#TotalDiscount").val(discountAmt);
    }

    var vatAmt = 0;
    if (vatPercent > 0) {
        vatAmt = (SubTotal * vatPercent) / 100;
    }
    if (isVatLine) {
        $("#LineVat").val(vatAmt);
    } else {
        $("#TotalVat").val(vatAmt);
    }   
}

function FxGroundTotal(saleAmount) {
    var SubTotal = saleAmount == "" ? 0 : parseFloat(saleAmount);
    var ShippingCharge = $("#idShippingCharge").val() == "" ? 0 : parseFloat($("#idShippingCharge").val());
    var Discount = $("#TotalDiscount").val() == "" ? 0 : parseFloat($("#TotalDiscount").val());
    var Vat = $("#TotalVat").val() == "" ? 0 : parseFloat($("#TotalVat").val());
    var GrandTotal = ((SubTotal - Discount) + (Vat + ShippingCharge));
    $("#idSubTotal").text(SubTotal.toFixed(2));
    $("#TotalDiscount").val(Discount.toFixed(2));
    $("#TotalVat").val(Vat.toFixed(2));
    $("#idGroundTotal").text(GrandTotal.toFixed(2));
}

function grandTotalCalculation() {
    var _SubTotal = $("#idSubTotal").text() == "" ? 0 : parseFloat($("#idSubTotal").text());
    var _ShippingCharge = $("#idShippingCharge").val() == "" ? 0 : parseFloat($("#idShippingCharge").val());
    var _TotalDiscount = $("#TotalDiscount").val() == "" ? 0 : parseFloat($("#TotalDiscount").val());
    var _TotalVat = $("#TotalVat").val() == "" ? 0 : parseFloat($("#TotalVat").val());
    $("#idGroundTotal").text(((_SubTotal + _ShippingCharge + _TotalVat) - _TotalDiscount).toFixed(2));
}

function calculateVatPercentage(totalAmount, totalVat) {
    var vatePercent = 0;
    if (!isNaN(totalAmount) && !isNaN(totalVat)) {
        vatePercent = totalVat * 100 / totalAmount;
    }
    return vatePercent;
}