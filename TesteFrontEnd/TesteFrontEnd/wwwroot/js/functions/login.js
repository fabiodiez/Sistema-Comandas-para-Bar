
function login() {
    var model = {};

    if ($("#username").val() == "") {
        alert("Digite o Usuário")
    }
    if ($("#password").val() == "") {
        alert("Digite a Senha");
    }

    model.username = $("#username").val();
    model.password = $("#password").val();


    $.ajax({
        type: "POST",
        url: url + "usuarios/login",
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
            localStorage.setItem("token", data.token);

            location.href = "/Comandas";
        },
        error: function (x, exception) {
            Object.keys(x).forEach(function (k) {
                if (k == "responseText") {
                    response = JSON.parse(x[k]);
                    alert(response.message);
                }
            });
        }
    });
}


function configurar() {
    $.get(url)
        .done(function (data) {
            itens = data;
        })
        .fail(function (data) {
            console.log("Erro: " + data.message);
        });
}
$(".nav-link").hide();
configurar()