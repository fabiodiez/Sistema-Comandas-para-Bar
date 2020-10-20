// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function validaLogin() {
    
    $.ajax({
        type: "POST",
        url: url + "usuarios/validatoken",        
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + localStorage.getItem("token")
        },
        success: function (data) {           
        },
        error: function (x, exception) {             
            location.href = "index";
        }
    });
}



$(".numero").mask("9999");
$(".valor").mask("999.99", { reverse: true });