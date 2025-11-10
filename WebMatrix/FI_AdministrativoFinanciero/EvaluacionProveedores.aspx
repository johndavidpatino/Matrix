<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EvaluacionProveedores.aspx.vb" Inherits="WebMatrix.EvaluacionProveedores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="UTF-8" />
    <script src="https://cdn.freecodecamp.org/testable-projects-fcc/v1/bundle.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.1.0/css/all.css" integrity="sha384-lKuwvrZot6UHsBSfcMvOkWwlCMgc0TaWr+30HWe3a4ltaBwTZhyTEggF5tJv8tbt" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css?family=Comfortaa|Lobster|Merienda|Merienda+One|Orbitron|Quicksand|Raleway" rel="stylesheet" />
    <title>Evaluación de Proveedores</title>
    <style>
        body {
            font-family: "Quicksand", sans-serif;
            background: rgba(52, 58, 64, 1);
            font-size: 16px;
        }

        #content {
            background: linear-gradient(to right, rgba(202, 240, 174, 1), rgba(144, 221, 196, 1));
            margin-top: 100px;
            border-radius: 10px;
            padding: 20px;
            border: 1px solid #fff;
            box-shadow: 2px 2px 10px #000;
        }

        #title {
            font-size: 48px;
            color: rgba(52, 58, 64, 0.8);
        }

        #description {
            border-radius: 3px;
            background: rgba(255, 255, 255, 0.5);
        }

        #info,
        #questionnaire {
            border-right: 1px solid #fff;
        }

        .form-control {
            font-size: 14px;
            margin-bottom: 10px;
            padding: 10px;
            border: 1px solid #ced4da;
            border-radius: 5px;
            width: 100%;
            box-sizing: border-box;
        }

        .custom-control {
            margin-bottom: 10px;
        }

        .custom-control-input {
            margin-right: 10px;
        }

        .custom-control-label {
            display: inline-block;
            margin-bottom: 0;
        }

        input:hover,
        textarea:hover,
        select:hover {
            border: 1px solid rgba(67, 198, 172, 1);
        }

        #yes {
            font-weight: bold;
        }

        #submitHelp {
            color: #555;
            font-size: 10px;
        }

        #submit {
            margin-top: 5px;
            font-size: 14px;
        }

        #footer {
            background: rgba(0, 0, 0, 0.5);
        }

            #footer h6 {
                margin-bottom: 0;
                font-size: 12px;
                color: #999;
            }

                #footer h6 a {
                    color: rgba(144, 221, 196, 1);
                    transition: 0.2s;
                }

                    #footer h6 a:hover {
                        color: rgba(202, 240, 174, 1);
                        transition: 0.2s;
                    }

        .inline-group {
            display: flex;
            align-items: center;
        }

            .inline-group label {
                margin-right: 10px; /* Ajusta este valor según sea necesario */
                flex: 2; /* Esto hará que el label tome el espacio necesario */
            }

            .inline-group .form-control {
                flex: 2; /* Este valor puede ser ajustado para controlar el tamaño del dropdown */
            }

        .btn {
            display: inline-block;
            font-weight: 400;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            user-select: none;
            border: 1px solid transparent;
            padding: 0.375rem 0.75rem;
            font-size: 1rem;
            line-height: 1.5;
            border-radius: 0.25rem;
            transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
        }

        .btn-primary {
            color: #fff;
            background-color: #007bff;
            border-color: #007bff;
        }

            .btn-primary:hover {
                color: #fff;
                background-color: #0056b3;
                border-color: #004085;
            }

            .btn-primary:focus, .btn-primary.focus {
                outline: 0;
                box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.5);
            }

            .btn-primary:active, .btn-primary.active,
            .show > .btn-primary.dropdown-toggle {
                color: #fff;
                background-color: #0056b3;
                border-color: #004085;
            }

                .btn-primary:active:focus, .btn-primary.active:focus,
                .show > .btn-primary.dropdown-toggle:focus {
                    box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.5);
                }

            .btn-primary.disabled, .btn-primary:disabled {
                color: #fff;
                background-color: #007bff;
                border-color: #007bff;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div id="content">
                <h1 id="title" class="text-center">Evaluación de Proveedores</h1>
                <p id="description" class="text-center">Agradecemos su colaboración diligenciando la evaluación para los proveedores con los que ha tenido relación en el último año. Diligencie una evaluación por cada proveedor que le haya sido enviado.</p>
                <div id="info" class="col">
                    <div style="clear: both">
                        <div class="form-group" style="float: left; width: 48%">
                            <label id="name-label" for="txtNombreProveedor">Proveedor</label>
                            <asp:TextBox ID="txtNombreProveedor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group" style="float: left; width: 48%; margin-left: 2%">
                            <label id="email-label" for="email">Symphony Center</label>
                            <asp:TextBox type="email" id="txtSymphonyCenter" class="form-control"  runat="server" ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <h2>Califique la calidad del servico prestado de este  proveedor en el ultimo año</h2>
                <div>
                    <div style="float: left; width: 48%">
                        <div class="form-group inline-group">
                            <label for="Q1">1. En general, ¿en qué medida está satisfecho con la calidad de los materiales y/o información que recibió del proveedor?</label>
                            <asp:DropDownList ID="Q1" CssClass="form-control" runat="server">
                                <asp:ListItem Value="10" Text="10 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; Cumplió totalmente"></asp:ListItem>
                                <asp:ListItem Value="9" Text="9 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="8" Text="8 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4 &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 &#9733; No cumplió en absoluto"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group inline-group">
                            <label for="Q2">2. ¿La comunicación con el proveedor fue acertada y oportuna?</label>
                            <asp:DropDownList ID="Q2" CssClass="form-control" runat="server">
                                <asp:ListItem Value="10" Text="10 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; Cumplió totalmente"></asp:ListItem>
                                <asp:ListItem Value="9" Text="9 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="8" Text="8 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4 &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 &#9733; No cumplió en absoluto"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group inline-group">
                            <label for="Q3">3. ¿Se cumplieron los tiempos establecidos para el desarrollo de los estudios o servicios?</label>
                            <asp:DropDownList ID="Q3" CssClass="form-control" runat="server">
                                <asp:ListItem Value="10" Text="10 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; Cumplió totalmente"></asp:ListItem>
                                <asp:ListItem Value="9" Text="9 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="8" Text="8 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4 &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 &#9733; No cumplió en absoluto"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="float: left; width: 48%; margin-left: 2%">
                        <div class="form-group inline-group">
                            <label for="Q4">4. ¿Se cumplieron los objetivos establecidos en el contrato?</label>
                            <asp:DropDownList ID="Q4" CssClass="form-control" runat="server">
                                <asp:ListItem Value="10" Text="10 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; Cumplió totalmente"></asp:ListItem>
                                <asp:ListItem Value="9" Text="9 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="8" Text="8 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4 &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 &#9733; No cumplió en absoluto"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group inline-group">
                            <label for="Q5">5. ¿Los entregables previstos fueron proporcionados con calidad?</label>
                            <asp:DropDownList ID="Q5" CssClass="form-control" runat="server">
                                <asp:ListItem Value="10" Text="10 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; Cumplió totalmente"></asp:ListItem>
                                <asp:ListItem Value="9" Text="9 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="8" Text="8 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4 &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 &#9733; No cumplió en absoluto"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group inline-group">
                            <label for="Q6">6. Se Cumplen los  de estándares vistos en el video de Evaluacion norma ISO 9001 – 20252 requeridos.</label>
                            <asp:DropDownList ID="Q6" CssClass="form-control" runat="server">
                                <asp:ListItem Value="10" Text="10 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; Cumplió totalmente"></asp:ListItem>
                                <asp:ListItem Value="9" Text="9 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="8" Text="8 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6 &#9733; &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 &#9733; &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4 &#9733; &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 &#9733; &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 &#9733; &#9733;"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 &#9733; No cumplió en absoluto"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>


                <div id="questionnaire" style="clear: both; text-align: center; width: 50%;" class="form-group inline-group">
                    <label for="rbContinue">Teniendo en cuenta las calificaciones asignadas, Usted considera que debemos continuar con los servicios de este proveedor</label>
                    <asp:RadioButtonList ID="rbQ7" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="True" Text="Si" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="False" Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <asp:Button ID="Volver" runat="server" Text="Regresar a Matrix" CssClass="btn btn-primary" OnClick="Volver_Click" />
                <asp:Button ID="Submit" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="Submit_Click" />
            </div>
        </div>
        <div id="footer" class="fixed-bottom text-center">
            <h6>Diseñado por IT para <a id="footbar">Gerencia Administrativa Área Compras</a> | <a>Ipsos Napoleón Franco SAS</a>
            </h6>
        </div>
    </form>
</body>
</html>
