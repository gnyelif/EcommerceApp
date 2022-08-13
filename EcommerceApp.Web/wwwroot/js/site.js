

$(document).ready(function () {
    $('table tr [name=drpOrderStatus]').each(function (index, value) {
        $(this).val($(this).attr('statusid')).attr("selected", "selected");
    });
    
});
function focusOn(element) {
    $(element).focus();
}

function addNewRow() {

    var rowHtml = "<tr><td> <div class='form-group'><label for= 'customerOrder'> Müşteri Sipariş No</label ><button type='button' class='btn btn-danger btn-right btn-xs btnDeleteRow' style='float: right;width:35px;height:35px;margin:4px' title='Sil'> X </button><input type='text' class='form-control txtCustomerOrderNo' id='customerOrder' placeholder='Müşteri Sipariş No' maxlength='10'></div> <div class='form-row'> <div class='form-group col-md-6'><label for='inputAddress1'>Çıkış Adresi</label> <input type='text' class='form-control txtSenderAddress' id='inputAddress1' placeholder='Çıkış Adresi' maxlength='250'></div>  <div class='form-group col-md-6'> <label for='inputAddress2'>Varış Adresi</label><input type='text' class='form-control txtDestinationAddress' id='inputAddress2' placeholder='Varış Adresi'  maxlength='250'></div> </div> <div class='form-row'>  <div class='form-group col-md-6'> <label for='inputQuantity'>Miktar</label><input class='form-control txtQuantity' id='inputQuantity' placeholder='Miktar' maxlength='5' type='number'></div> <div class='form-group col-md-6'><label for='inputQuantityUnit'>Miktar Birim</label> <select id='inputQuantityUnit' class='form-control quantityUnit'><option selected>Seçiniz</option> <option>Adet</option><option>Koli</option> <option>Paket</option><option>Palet</option> </select>  </div></div> <div class='form-row'><div class='form-group col-md-6'><label for='inputWeight'>Ağırlık</label><input class='form-control txtWeight' id='inputWeight' placeholder='Ağırlık' maxlength='5'' type='number'></div> <div class='form-group col-md-6'> <label for='inputWeightUnit'>Ağırlık Birim</label><select id='inputWeightUnit' class='form-control weightUnit'><option selected>Seçiniz</option><option>Kg</option><option>Ton</option></select></div></div><div class='form-row'><div class='form-group col-md-6'><label for='materialCode'>Malzeme Kodu</label><input type='text' class='form-control txtMaterialCode' id='materialCode' placeholder='Malzeme Kodu'  maxlength='10'></div> <div class='form-group col-md-6'> <label for='materialName'>Malzeme Adı</label><input type='text' class='form-control txtMaterialName' id='materialName' placeholder='Malzeme Adı'  maxlength='50'></div> </div> <div class='form-group'> <label for='not'>Not</label>  <input type='text' class='form-control txtNot' id='not' placeholder='Not'  maxlength='250'></div><hr></td></tr>";

    $('.tblAddOrder').append(rowHtml);
}

$("[name=drpOrderStatus]").change(function () {
    var selectedStatusCode = $(this).val();
    var selectedOrderId = $(this).attr('orderId');

    if (selectedStatusCode !== "" && selectedOrderId !== "") {
        swal({
            title: "",
            text: "Siparişin durumunu değiştirmek istediğinizden emin misiniz ?",
            //icon: "warning",
            buttons: true,
            dangerMode: true,
            buttons: ["İptal", "Evet"],
        })
            .then((willDelete) => {
                if (willDelete) {

                    $.ajax({
                        type: "post",
                        url: "/Home/UpdateStatus/",
                        data: { "statusCode": selectedStatusCode, "orderId": selectedOrderId },
                        datatype: "json",
                        traditional: true,
                        error: function (jqXHR, textStatus, errorThrown) {
                            alert(jqXHR + "-" + textStatus + "-" + errorThrown);
                        },
                        success: function (data) {
                            if (data.status == 0) {

                                swal({
                                    position: 'center',
                                    type: 'error',
                                    title: "Güncelleme Başarılı ! ",
                                    closeonclickoutside: true,
                                    allowoutsideclick: true,
                                    confirmbuttontext: "Ok"
                                });
                                $('.swal-title').css('font-size', '20px');


                            }
                            else {
                                swal({
                                    position: 'center',
                                    type: 'error',
                                    title: "Güncelleme Başarısız ! ",
                                    closeonclickoutside: true,
                                    allowoutsideclick: true,
                                    confirmbuttontext: "Ok"
                                });
                                $('.swal-title').css('font-size', '20px');
                            }
                        }
                    });
                }
            });
    }
});

$(document).on("click", ".btnDeleteRow", function () {

    swal({
        title: "",
        text: "Sipariş satırını silmek istediğinize emin misiniz ?",
        // icon: "warning",
        confirmButtonText: "Select Patient?",
        cancelButtonText: "Speed Case?",
        buttons: true,
        dangerMode: true,
        buttons: ["İptal", "Evet"],
    })
        .then((willDelete) => {
            if (willDelete) {
                $(this).parent().parent().parent().remove();

            }
        });

})

$(document).on("click", ".btnAddNewRow", function () {
    addNewRow();
});
$(document).on("click", ".btnSaveOrder", function (event) {
    var itemList = [];
    var model = [];
    var out = true;

    $('table tr').each(function (index, value) {

        var cNo = $(this).find('.txtCustomerOrderNo').val();
        var address1 = $(this).find('.txtSenderAddress').val();
        var address2 = $(this).find('.txtDestinationAddress').val();
        var _quantity = $(this).find('.txtQuantity').val();
        var qUnit = $(this).find('.quantityUnit').val();
        var _weight = $(this).find('.txtWeight').val();
        var wUnit = $(this).find('.weightUnit').val();
        var mCode = $(this).find('.txtMaterialCode').val();
        var mName = $(this).find('.txtMaterialName').val();
        var _not = $(this).find('.txtNot').val();

        if (cNo == "" || cNo == undefined) {
            focusOn(".txtCustomerOrderNo");
            out = false;
        }
        if (_quantity == "" || _quantity == undefined) {
            focusOn(".txtQuantity");
            out = false;
        }
        if (_weight == "" || _weight == undefined) {
            focusOn(".txtWeight");
            out = false;
        }
        if (mCode == "" || mCode == undefined) {
            focusOn(".txtMaterialCode");
            out = false;
        }

        if (qUnit == "" || qUnit == undefined || qUnit == "Seçiniz") {
            focusOn(".quantityUnit");
            out = false;
        }

        if (wUnit == "" || wUnit == undefined || wUnit == "Seçiniz") {
            focusOn(".weightUnit");
            out = false;
        }




        var _itemList = {
            customerorderno: cNo, senderaddress: address1, destinationaddress: address2,
            quantity: _quantity,
            quantityunit: qUnit,
            weight: _weight, weightunit: wUnit,
            materialcode: mCode, materialname: mName, not: _not
        };
        model.push(_itemList);
    });

    if (!out) { event.preventDefault(); }
    else {
        $.ajax({
            type: "post",
            url: "/Home/AddItemList",
            datatype: "json",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR + "-" + textStatus + "-" + errorThrown);
            },
            success: function (data) {
                var result = [];
                var text = "";
                if (data.filter(a => a.status == "1").length != 0) {
                    result = data.filter(a => a.status == "1");

                    for (var i = 0; i < result.length; i++) {
                        text = text + result[i].statusMessage + " ";
                    }

                }
                else {
                    text = "Siparişler başarılı kaydedilmiştir ";
                }

                swal({
                    position: 'center',
                    type: 'error',
                    title: text,
                    closeonclickoutside: true,
                    allowoutsideclick: true,
                    confirmbuttontext: "Ok"
                });
                $('.swal-title').css('font-size', '20px');

            }
        });
    }

});



