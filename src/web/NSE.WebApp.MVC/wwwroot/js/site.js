function BuscaCep() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            $("#Address_Street").val("");
            $("#Address_Neighborhood").val("");
            $("#Address_City").val("");
            $("#Address_State").val("");
        }

        $("#Address_ZipCode").blur(function () {

            var cep = $(this).val().replace(/\D/g, '');

            if (cep != "") {

                var validacep = /^[0-9]{8}$/;

                if (validacep.test(cep)) {

                    $("#Address_Street").val("...");
                    $("#Address_Neighborhood").val("...");
                    $("#Address_City").val("...");
                    $("#Address_State").val("...");

                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                $("#Address_Street").val(dados.logradouro);
                                $("#Address_Neighborhood").val(dados.bairro);
                                $("#Address_City").val(dados.localidade);
                                $("#Address_State").val(dados.uf);
                            } 
                            else {
                                limpa_formulário_cep();
                                alert("Zip Code not found.");
                            }
                        });
                }
                else {
                    limpa_formulário_cep();
                    alert("Invalid Zip Code format.");
                }
            }
            else {
                limpa_formulário_cep();
            }
        });
    });
}