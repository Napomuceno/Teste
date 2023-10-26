using System;
using System.Drawing;
using Servicelogic.TMS.Administracao;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicelogic.TMS.Carga;
using Servicelogic.TMS.Basico;
using System.Data;
using Servicelogic.TMS.Carga.Rel;
using Servicelogic.TMS.Container;
using Servicelogic.TMS.Web.Controles;
using System.Web;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Servicelogic.TMS.Web.Carga.Pagina
{
    public partial class _ConhecimentoOtimizado : System.Web.UI.Page
    {
        ConhecimentoOtimizado objConhecimentoOtimizado = new ConhecimentoOtimizado();
        ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
        ServiceLogic.Tool.Tools _objTools = new ServiceLogic.Tool.Tools();

        #region[Propriedades]
        private int Id
        {
            get;
            set;
        }
        private DataTable DataTableTabelaFrete
        {
            get
            {
                return (DataTable)ViewState["DataTableTabelaFrete"];
            }
            set
            {
                ViewState["DataTableTabelaFrete"] = value;
            }
        }

        private static int IdNotaFiscal
        {
            get;
            set;
        }

        TiposConstantes objTiposConstantes = new TiposConstantes();
        ClassSQLBasico objClassSQLBasico = new ClassSQLBasico();
        ClassSQLAdministracao objClassSQLAdministracao = new ClassSQLAdministracao();
        ClassSQLCarga objClassSQLCarga = new ClassSQLCarga();

        string caminho = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"] + @"\AreaTransferencia\NFTerceiro" + "\\";
        string caminhoCTETerceiro = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"] + @"\AreaTransferencia\CTETerceiro" + "\\";

        private ArrayList ImportaXMLNFeList
        {
            get
            {
                try
                {
                    if (ViewState["ImportaXMLNFeList"] == null)
                        ViewState["ImportaXMLNFeList"] = new ArrayList();

                    return (ArrayList)(ViewState["ImportaXMLNFeList"]);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["ImportaXMLNFeList"] = value;
            }
        }

        private ArrayList TextBoxList
        {
            get
            {
                try
                {
                    if (ViewState["TextBoxList"] == null)
                        ViewState["TextBoxList"] = new ArrayList();

                    return (ArrayList)(ViewState["TextBoxList"]);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["TextBoxList"] = value;
            }
        }

        private const int _NumMaxRegistrosConsulta = 1000; //Numero maximo de registros que a consulta trará por vez;

        private int IdNotaFiscalRemetente;

        #endregion

        #region [ Page Load ]
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.Page_Load(Page);

                ControlaExibicaoNumViagem();

                if (this.GridNotasFiscaisSelecionadas.Rows.Count != 0)
                {
                    aplicaMascaraPeso();
                }

                if (!IsPostBack)
                {
                    this.mvwPrincipal.ActiveViewIndex = 0;
                    this.mvwGeral.ActiveViewIndex = 0;



                    ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
                    objConfiguraModulo.Get();
                    if (objConfiguraModulo.ExibirNumProgramacaoCadastroOtimizado == (byte)eSimNao.Sim)
                        this.PnlProgramacao.Visible = true;

                    this.PnlRotaCTe.Visible = objConfiguraModulo.UtilizarControleRotaCadastroOtimizado == (byte)eSimNao.Sim;
                    this.PnlRotaCTe.Enabled = objConfiguraModulo.UtilizarControleRotaCadastroOtimizado == (byte)eSimNao.Sim;

                    this.pnlTipoOperacaoTransporte.Visible = objConfiguraModulo.PermitirDefinicaoOperacaoTransporteCadastroOtimizadoCTe == (byte)eSimNao.Sim;
                    this.pnlTipoOperacaoTransporte.Enabled = objConfiguraModulo.PermitirDefinicaoOperacaoTransporteCadastroOtimizadoCTe == (byte)eSimNao.Sim;

                    this.ChkConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05.Visible = objConfiguraModulo.NaGeracaoCteConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05 == (byte)eSimNao.Sim;

                    this.configuracaoComponentes();
                    this.atualizarGridComponentesCubagem();

                    AtualizarCampoVeiculo();
                    tratarCamposPallet();
                    this.TrataVisualGridNotasCTe();
                    FiltraPesquisa();
                    controlarVisibilidadeLinkTabelaFrete();
                    this.DataTableTabelaFrete = new DataTable();
                    setInfoFreteGeralM03();



                }
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        private void AtualizarCampoVeiculo()
        {
            ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
            objConfiguraModulo.Get();

            if (objConfiguraModulo.ExigirVeiculoEmConhecimento == (byte)eSimNao.Sim)
            {
                this.TxtPlaca_Veiculo_ConhecimentoOtimizado.CssClass = "ctxtObr";
                this.TextBoxWatermarkExtender4.WatermarkCssClass = "ctxtObr";
            }
            else
            {
                this.TxtPlaca_Veiculo_ConhecimentoOtimizado.CssClass = "ctxt";
                this.TextBoxWatermarkExtender4.WatermarkCssClass = "ctxt";
            }
        }

        private void AtualizaTipoTransporte(NotaFiscal objNotaFiscal)
        {
            ConhecimentoOtimizado objConhecimentoOtimizado = new ConhecimentoOtimizado();
            ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
            objConfiguraModulo.Get();

            if (objConfiguraModulo.CadastroOtimizadoCTeSomenteOpcoesDisponiveisDeAcordoComTabelaDeFrete == (byte)eSimNao.Sim)
            {
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.Items.Clear();
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.AppendDataBoundItems = false;
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.Items.Insert(0, new ListItem("Novo...", "0"));
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.AppendDataBoundItems = true;
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.DataTextField = "DescTipoTransporte";
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.DataValueField = "IdTipoTransporte";
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.DataSource = objConhecimentoOtimizado.TipoTransportePorNota(objNotaFiscal);
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.SelectedValue = "0";
                this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.DataBind();

            }

        }

        private void ControlaExibicaoNumViagem()
        {

            ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
            objConfiguraModulo.Get();
            if (objConfiguraModulo.PermitirGerarRelacionarViagemGeracaoCTeCadastroOtimizado == (byte)eSimNao.Sim)
            {
                this.PnlAcoesViagem.Visible = true;
                if (Rb2RelacionarViagem.Checked)
                {
                    TxtNumViagem_ConhecimentoOtimizado.Style.Add("display", "block");
                    LblRelacionarViagem.Style.Add("display", "block");
                }
                else
                {
                    TxtNumViagem_ConhecimentoOtimizado.Style.Add("display", "none");
                    LblRelacionarViagem.Style.Add("display", "none");
                }
            }
            else
                this.PnlAcoesViagem.Visible = false;

        }
        #endregion

        #region [ Metodos da geracao do Conhecimento ]

        #region[ Editar/Atualizar Coleta NF ]
        protected void LkbAtualizarColeta_Click(object sender, EventArgs e)
        {
            try
            {
                preencherMunicipio(TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado, TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado,
                    TxtUF_MunicipioColeta_ConhecimentoOtimizado);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[Veículo]
        protected void LkbAtualizarVeiculo_Click(object sender, EventArgs e)
        {
            try
            {
                preencherVeiculo(TxtCodVeiculo_Veiculo_ConhecimentoOtimizado, TxtPlaca_Veiculo_ConhecimentoOtimizado);
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbEditarVeiculo_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodVeiculo_Veiculo_ConhecimentoOtimizado, typeof(Veiculo), "Veiculo.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }
        #endregion

        #region[RotaCTe]
        protected void LkbAtualizarRotaCTe_Click(object sender, EventArgs e)
        {
            try
            {
                PaginaWeb.PreencherControleDropDown(Page, DdlIdRotaCTe_RotaCTe_ConhecimentoOtimizado);
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbEditarRotaCTe_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.DdlIdRotaCTe_RotaCTe_ConhecimentoOtimizado, "RotaCTe.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }
        #endregion

        #region [       Pagador       ]
        protected void LkbAtualizarPagador_Click(object sender, EventArgs e)
        {
            try
            {
                preencherCliente(this.TxtCodCliente_Pagador_ConhecimentoOtimizado, this.TxtNome_Pagador_ConhecimentoOtimizado);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void LkbEditarPagador_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodCliente_Pagador_ConhecimentoOtimizado, typeof(Cliente), "Cliente.aspx");
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region [ Liberar Pagador ]
        protected void CifFob_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RbtGeracaoCtePorNota.Checked)
                    this.TrataCifFobGeracaoCTePorNota(sender, e);
                else
                    this.TrataCifFobGeracaoCTePorCTeTerceiro(sender, e);


                this.TxtNome_Pagador_ConhecimentoOtimizado.Enabled = this.Rb3CifFob_ConhecimentoOtimizado.Checked ? true : false;
                this.LkbAtualizarPagador.Enabled = this.Rb3CifFob_ConhecimentoOtimizado.Checked ? true : false;
                this.LkbEditarPagador.Enabled = this.Rb3CifFob_ConhecimentoOtimizado.Checked ? true : false;
            }
            catch (Exception erro)
            {
                this.LblMensagem.ForeColor = Color.Red;
                this.LblMensagem.Text = erro.Message;
                return;
            }
        }

        protected void CifFob_ConfiguraModulo_Click(object sender, EventArgs e)
        {
            try
            {
                int idNotaFiscal = 0;
                this.Rb1CifFob_ConhecimentoOtimizado.Checked = false;
                this.Rb2CifFob_ConhecimentoOtimizado.Checked = false;
                this.Rb3CifFob_ConhecimentoOtimizado.Checked = false;

                DataTable dtTodasSelecionadas = ControleGridView.TransformarGridViewInDataTable(this.GridNotasFiscaisSelecionadas, false);
                if (dtTodasSelecionadas != null)
                {
                    if (dtTodasSelecionadas.Rows.Count > 0)
                        idNotaFiscal = Convert.ToInt32(dtTodasSelecionadas.Rows[0]["IdNotaFiscal"]);

                    dtTodasSelecionadas.Dispose();
                }

                NotaFiscal objNotaFiscal = new NotaFiscal();
                objNotaFiscal.GetById(idNotaFiscal);

                ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
                objConfiguraModulo.Get();

                if (objConfiguraModulo.SugerirTomadorFreteCadastroOtimizado == (byte)eSimNao.Sim)
                {
                    this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Remetente.CodCliente.ToString().Trim();
                    this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Remetente.CnpjCpf.ToString().Trim();
                    LkbAtualizarPagador_Click(sender, e);
                    this.Rb1CifFob_ConhecimentoOtimizado.Checked = true;
                }
                else
                {
                    if (objConfiguraModulo.SugerirTomadorFreteCadastroOtimizado == (byte)eSimNao.Nao)
                    {
                        this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Destinatario.CodCliente.ToString().Trim();
                        this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Destinatario.CnpjCpf.ToString().Trim();
                        LkbAtualizarPagador_Click(sender, e);
                        this.Rb2CifFob_ConhecimentoOtimizado.Checked = true;
                    }
                    else if (objConfiguraModulo.SugerirTomadorFreteCadastroOtimizado == (byte)3)
                    {
                        this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = "";
                        this.TxtNome_Pagador_ConhecimentoOtimizado.Text = "";
                        this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = "";
                        this.TxtNome_Pagador_ConhecimentoOtimizado.Enabled = true;
                        this.LkbAtualizarPagador.Enabled = true;
                        this.LkbEditarPagador.Enabled = true;
                        this.Rb3CifFob_ConhecimentoOtimizado.Checked = true;
                    }
                }

            }
            catch (Exception erro)
            {
                this.LblMensagem.ForeColor = Color.Red;
                this.LblMensagem.Text = erro.Message;
                return;
            }
        }

        protected void CifFob_NotaFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                int idNotaFiscal = 0;
                this.Rb1CifFob_ConhecimentoOtimizado.Checked = false;
                this.Rb2CifFob_ConhecimentoOtimizado.Checked = false;
                this.Rb3CifFob_ConhecimentoOtimizado.Checked = false;

                NotaFiscal objNotaFiscal = new NotaFiscal();
                ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
                objConfiguraModulo.Get();

                DataTable dtTodasSelecionadas = ControleGridView.TransformarGridViewInDataTable(this.GridNotasFiscaisSelecionadas, false);
                if (dtTodasSelecionadas != null)
                {
                    if (dtTodasSelecionadas.Rows.Count > 0)
                        idNotaFiscal = Convert.ToInt32(dtTodasSelecionadas.Rows[0]["IdNotaFiscal"]);

                    dtTodasSelecionadas.Dispose();
                }

                objNotaFiscal.GetById(idNotaFiscal);

                if (objConfiguraModulo.ConsiderarCifFobOriundoNotaFiscalCadastroOtimizadoCTE == (byte)eSimNao.Sim)
                {
                    if (objNotaFiscal.CifFob == 'C' || objNotaFiscal.CifFob == '0' || objNotaFiscal.CifFob == '1')
                    {
                        this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Remetente.CodCliente.ToString().Trim();
                        this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Remetente.CnpjCpf.ToString().Trim();
                        LkbAtualizarPagador_Click(sender, e);
                        this.Rb1CifFob_ConhecimentoOtimizado.Checked = true;
                        this.Rb1CifFob_ConhecimentoOtimizado.Enabled = false;
                        this.Rb2CifFob_ConhecimentoOtimizado.Enabled = false;
                        this.Rb3CifFob_ConhecimentoOtimizado.Enabled = false;
                    }
                    else if (objNotaFiscal.CifFob == 'F')
                    {
                        this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Destinatario.CodCliente.ToString().Trim();
                        this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Destinatario.CnpjCpf.ToString().Trim();
                        LkbAtualizarPagador_Click(sender, e);
                        this.Rb2CifFob_ConhecimentoOtimizado.Checked = true;
                        this.Rb1CifFob_ConhecimentoOtimizado.Enabled = false;
                        this.Rb2CifFob_ConhecimentoOtimizado.Enabled = false;
                        this.Rb3CifFob_ConhecimentoOtimizado.Enabled = false;
                    }
                }
            }
            catch (Exception erro)
            {
                this.LblMensagem.ForeColor = Color.Red;
                this.LblMensagem.Text = erro.Message;
                return;
            }
        }

        #endregion

        #region [ Editar nota fiscal ]
        protected void EditarNotaFiscal_Command(object sender, CommandEventArgs e)
        {
            try
            {
                NotaFiscal objNotaFiscal = new NotaFiscal();
                IdNotaFiscal = Convert.ToInt32(e.CommandArgument.ToString());
                objNotaFiscal.GetById(IdNotaFiscal);
                ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
                objConfiguraModulo.Get();
                if (objConfiguraModulo.NaoPermitirEditarDocFiscalAutorizadoSefaz == (byte)eSimNao.Sim)
                {
                    ClassSQLCarga objClassSQLCarga = new ClassSQLCarga();
                    if (objClassSQLCarga.GetByIdNotaFiscalContidaConhecimentoAutorizadoSefaz(IdNotaFiscal, 0))
                        PaginaWeb.Mensagem(Page, "Alteração da Nota não permitida em Documneto Fiscal já autorizado.", TipoMensagem.Error);
                }
                ControlesWeb.PreencheWebControls(objNotaFiscal, Page);
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 1;
                this.SMProgramacao.SetFocus(this.DdlIdFilial_Filial_NotaFiscal.ClientID);

                //ScriptManager.RegisterStartupScript(this, typeof(Page), "NotaFiscal", "<script type='text/javascript' language='javascript'>Editar('" + _idNota.ToString() + "','../NotaFiscal/NotaFiscal.aspx');</script>", false);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region [ Move notas disponiveis para selecionadas ]
        protected void BtnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RbtGeracaoCtePorNota.Checked)
                    this.TrataGeracaoCTePorNota(sender, e);
                else
                {

                    this.TrataGeracaoCTePorCTeTerceiro(sender, e);
                }

                this.TrataVisualGridNotasCTe();

            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        private void verificarNotaJaVinculadaConhecimentoMesmoTipo()
        {
            ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
            objConfiguraModulo.Get();

            if (objConfiguraModulo.SolicitarLiberacaoNotaReutilizacaoConhecimentoMesmoTipo != (byte)eSimNao.Sim)
                return;

            string idsNotasFiscais = "";
            ClassSQLCarga objClassSQLCarga = new ClassSQLCarga();
            LiberacaoNotaFiscalReutilizacaoCTRC objLiberacaoNotaFiscalReutilizacaoCTRC = new LiberacaoNotaFiscalReutilizacaoCTRC();

            DataTable dtSelecionadas = ControleGridView.TransformarGridViewInDataTable(this.GvwListaNotasDisponiveis, true);


            for (int i = 0; i < dtSelecionadas.Rows.Count; i++)
                idsNotasFiscais = objClassSQLCarga.Tools.IsNullOrEmpty(idsNotasFiscais) ? dtSelecionadas.Rows[i]["IdNotaFiscal"].ToString() : idsNotasFiscais + ", " + dtSelecionadas.Rows[i]["IdNotaFiscal"];

            NotaFiscalList objNotaFiscalList = new NotaFiscalList();
            objNotaFiscalList.PreencherLista(typeof(NotaFiscal), " SELECT * FROM NotaFiscal WHERE IdNotaFiscal in(" + idsNotasFiscais + ") ");

            objLiberacaoNotaFiscalReutilizacaoCTRC.ValidarNotaFiscalConhecimentoMesmoTipo(objNotaFiscalList, (byte)eTipoConhecimento.Normal);

        }

        #region [ Move notas selecionadas para disponiveis ]
        protected void BtnRemover_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RbtGeracaoCtePorNota.Checked)
                {

                    ClassSQLCarga.stParametros objparam = PopularObjParametros();
                    ControleGridView.MigrarSelecionadasChecadasParaDisponiveis(this.GridNotasFiscaisSelecionadas, this.GvwListaNotasDisponiveis, this.DdlTipoConsulta.SelectedValue.ToString(), objparam, _NumMaxRegistrosConsulta, true);
                    AtualizarControlesNFsRelacionadas();
                    computarTotalPalletsNotaNaturezaPaletizada();

                    if (this.GridNotasFiscaisSelecionadas.Rows.Count == 0)
                        DdlTipoOperacaoTransporte_ConhecimentoOtimizado.Enabled = pnlTipoOperacaoTransporte.Enabled;
                }
                else
                {

                    ClassSQLCarga.stParametrosCTeTerceiro objparame = PopularObjParametrosCTeTerceiro();
                    ControleGridView.MigrarCteTerceiroSelecionadasChecadasParaDisponiveis(this.GvwListaCTeTerceiroSelecionadas, this.GvwListaCTeTerceiroDisponiveis, this.DdlTipoConsulta.SelectedValue.ToString(), objparame, _NumMaxRegistrosConsulta, true);



                }

                this.AtualizarInformacoesControlesWeb();
                this.TrataVisualGridNotasCTe();
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region [ Sair ]
        protected void BtnSair_Click(object sender, EventArgs e)
        {
            Session["Novo"] = "Nao";
            Session["ConhecimentoOtimizado"] = "1";
            ControlesWeb.RedirecionarSimples(Page, "Conhecimento.aspx", "id=0&sairotimizado=S");
        }
        #endregion

        #region [ Gerar Conhecimento ]
        protected void GerarCTRC(object sender, EventArgs e)
        {
            try
            {

                if (Rb2RelacionarViagem.Checked && (String.IsNullOrEmpty(TxtNumViagem_ConhecimentoOtimizado.Text) || TxtNumViagem_ConhecimentoOtimizado.Text == "0"))
                    throw new Exception("Informe o Número da Viagem!");

                ControlaOpcaoGeracaoViagemSelecionada();

                if (this.RbtGeracaoCtePorNota.Checked)
                {

                    ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
                    objConfiguraModulo.Get();

                    //ControlesWeb.DesabilitarPagina(Page);
                    this.BtnAddComponente.Enabled = false;
                    NotaFiscalList objNotaFiscalList = ControleGridView.getNotaFiscalList(this.GridNotasFiscaisSelecionadas);
                    if (this.verifcarControlePallet())
                        throw new Exception("Não existe nota fiscal selecionada que tenha natureza com a característica Controla Pallet. Verifique o cadastro da nota fiscal.");

                    ControlesWeb.EnviarCamposParaInstancia(Page, objConhecimentoOtimizado);
                    PaginaWeb.preencherCamposAuditoria(objConhecimentoOtimizado, Page);
                    Cliente objPagador = new Cliente();

                    int codigocliente = String.IsNullOrEmpty(this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text) ? 0 : Convert.ToInt32(this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text);
                    objPagador.GetBy(" CodCliente = " + codigocliente.ToString().Trim());
                    Veiculo objVeiculo = new Veiculo();
                    if (!String.IsNullOrEmpty(TxtPlaca_Veiculo_ConhecimentoOtimizado.Text))
                    {

                        if (objVeiculo.GetBy("Placa = '" + this.TxtPlaca_Veiculo_ConhecimentoOtimizado.Text + "'"))
                            objConhecimentoOtimizado.Veiculo = objVeiculo;

                        if (objConfiguraModulo.BloquearLancamentoViagemAutorizacaoCTeParaVeiculosComManutencaoVencida == (byte)eSimNao.Sim)
                        {
                            string retorno = objClassSQLBasico.BuscaVeiculoxManutencaoVencida(objVeiculo.IdVeiculo);
                            if (retorno.Contains("Vencida"))
                            {
                                throw new Exception(retorno);
                            }
                        }
                    }

                    if (objConhecimentoOtimizado.Veiculo.IdMotorista == 0 && Rb1GerarViagem.Checked)
                        throw new Exception("Veículo sem Motorista!");

                    int _IdTabelaFreteRateamento = getIdTabelaFreteRateamento();

                    objConhecimentoOtimizado.NumProgramacao = _objTools.ConvertToInt32(this.TxtNumProgramacao_ConhecimentoOtimizado.Text);

                    objConhecimentoOtimizado.IdPagador = objPagador.IdCliente;

                    objConhecimentoOtimizado.IdConhecimentoOtimizado = objConhecimentoOtimizado.GerarCTRC(objNotaFiscalList, this.ckbGerarCTRB_ConhecimentoOtimizado.Checked, this.ChkConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05.Checked, _IdTabelaFreteRateamento);
                }
                else
                {

                    ControlesWeb.EnviarCamposParaInstancia(Page, objConhecimentoOtimizado);
                    PaginaWeb.preencherCamposAuditoria(objConhecimentoOtimizado, Page);
                    Cliente objPagador = new Cliente();

                    int codigocliente = String.IsNullOrEmpty(this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text) ? 0 : Convert.ToInt32(this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text);
                    objPagador.GetBy(" CodCliente = " + codigocliente.ToString().Trim());
                    Veiculo objVeiculo = new Veiculo();
                    if (!String.IsNullOrEmpty(TxtPlaca_Veiculo_ConhecimentoOtimizado.Text))
                    {

                        if (objVeiculo.GetBy("Placa = '" + this.TxtPlaca_Veiculo_ConhecimentoOtimizado.Text + "'"))
                            objConhecimentoOtimizado.Veiculo = objVeiculo;

                        if (objConfiguraModulo.BloquearLancamentoViagemAutorizacaoCTeParaVeiculosComManutencaoVencida == (byte)eSimNao.Sim)
                        {
                            string retorno = objClassSQLBasico.BuscaVeiculoxManutencaoVencida(objVeiculo.IdVeiculo);
                            if (retorno.Contains("Vencida"))
                            {
                                throw new Exception(retorno);
                            }
                        }
                    }
                    objConhecimentoOtimizado.IdPagador = objPagador.IdCliente;

                    ConhecimentoTerceiroList objConhecimentoTerceiroList = ControleGridView.GetConhecimentoTerceiroList(this.GvwListaCTeTerceiroSelecionadas);
                    objConhecimentoOtimizado.IdConhecimentoOtimizado = objConhecimentoOtimizado.GerarCTRC(null, this.ckbGerarCTRB_ConhecimentoOtimizado.Checked, this.ChkConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05.Checked, 0, false, null, objConhecimentoTerceiroList);
                }


                this.salvarCTRCxComponente();

                if (objConhecimentoOtimizado.ListConhecimentoGerados.Count > 1)
                {
                    Session["MensagemOtimizado"] = "Conhecimentos gerados: " + objConhecimentoOtimizado.GetNumerosCTRCGerados();
                }
                else
                {
                    Session["MensagemOtimizado"] = null;
                }


                Session["ConhecimentoOtimizado"] = "1";

                if (!Rb2Nenhum.Checked)
                {
                    if (objConhecimentoOtimizado.GerarViagem && objConhecimentoOtimizado.NumViagem > 0)
                        Session["NumViagemGerada"] = objConhecimentoOtimizado.NumViagem;
                    else
                        Session["NumViagemRelacionada"] = objConhecimentoOtimizado.NumViagem;
                }
                else
                {
                    Session["NumViagemGerada"] = null;
                    Session["NumViagemRelacionada"] = null;
                }

                ControlesWeb.Redirecionar(Page, "Conhecimento.aspx", objConhecimentoOtimizado.IdConhecimentoOtimizado);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
            finally
            {
                tratarCamposPallet();
                this.BtnAddComponente.Enabled = true;
            }
        }

        private void ControlaOpcaoGeracaoViagemSelecionada()
        {
            if (Rb1GerarViagem.Checked)
            {
                objConhecimentoOtimizado.GerarViagem = true;
                objConhecimentoOtimizado.NaoGerarViagem = false;
            }
            else if (Rb2Nenhum.Checked)
            {
                objConhecimentoOtimizado.NaoGerarViagem = true;
                objConhecimentoOtimizado.GerarViagem = false;
            }
            else //Relacionar Viagem
            {
                objConhecimentoOtimizado.NaoGerarViagem = false;
                objConhecimentoOtimizado.GerarViagem = false;
            }
        }
        #endregion

        #region [ Atualizar labels e edits ]
        private void AtualizarInformacoesControlesWeb()
        {
            if (this.RbtGeracaoCtePorNota.Checked)
            {
                this.AtualizarInformacoesControlesWebCTePorNota();
            }
            else
                this.AtualizarInformacoesControlesWebCTePorCTeTerceiro();


        }

        #endregion

        #endregion

        #region [ Metodos do cadastro de notas novas ]
        protected void BtnAbrirOpcoesConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                if (mvwPrincipal.ActiveViewIndex == 1)
                    BtnAbrirImportacaoXML_Click(sender, e);

                limparNotaFiscal();
                IdNotaFiscal = 0;
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 1;
                this.SMProgramacao.SetFocus(this.DdlIdFilial_Filial_NotaFiscal.ClientID);


            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void BtnAbrirCadastroOtimizado_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RbtGeracaoCtePorNota.Checked)
                {
                    
                    this.DdlTipoConsulta.SelectedIndex = 0;                    
                    ClassSQLCarga.stParametros objstParam = PopularObjParametros();
                    ControleGridView.CarregarNotasDisponiveisBD(this.GvwListaNotasDisponiveis, this.GridNotasFiscaisSelecionadas, this.DdlTipoConsulta.SelectedValue, objstParam, _NumMaxRegistrosConsulta);
                    AtualizarInformacoesControlesWeb();
                    this.mvwPrincipal.ActiveViewIndex = 0;
                    this.mvwGeral.ActiveViewIndex = 0;
                    BtnLimparConsultaNotaFiscal_Click(sender, e);
                }
                else
                {
                    this.DdlTipoConsultaCteTerceiro.SelectedIndex = 0;
                    ClassSQLCarga.stParametrosCTeTerceiro objstParametro = PopularObjParametrosCTeTerceiro();
                    ControleGridView.CarregarConhecimentoTerceiroDisponiveisBD(this.GvwListaCTeTerceiroDisponiveis, this.GvwListaCTeTerceiroSelecionadas, this.DdlTipoConsultaCteTerceiro.SelectedValue, objstParametro, _NumMaxRegistrosConsulta);
                    /*this.RbtGeracaoCtePorNota.Checked = true;
                    this.TrataVisualGridNotasCTe();*/                   
                    AtualizarInformacoesControlesWeb();
                    this.mvwPrincipal.ActiveViewIndex = 0;
                    this.mvwGeral.ActiveViewIndex = 0;
                    BtnLimparConsultaCteTerceiro_Click(sender, e);

                }

            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        #region[ Editar/Atualizar Modelo NF ]
        protected void LkbEditaModeloNF_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(DdlIdModeloNF_NotaFiscal, "CadastroPreDefinido.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarModeloNF_Click(object sender, EventArgs e)
        {
            try
            {
                PaginaWeb.PreencherControleDropDown(this.Page, DdlIdModeloNF_NotaFiscal);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[ Editar/Atualizar Natureza ]
        protected void LkbEditarNatureza_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodNatureza_Natureza_NotaFiscal, typeof(Natureza), "Natureza.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarNatureza_Click(object sender, EventArgs e)
        {
            try
            {

                string codNatureza = this.TxtCodNatureza_Natureza_NotaFiscal.Text;
                if (objClassSQLBasico.Tools.FieldString(codNatureza) == "")
                {
                    PaginaWeb.PreencherControleDropDown(this.Page, DdlIdNatureza_Natureza_XML);
                }
                else
                {
                    Natureza objNatureza = new Natureza();
                    objNatureza.GetBy("CodNatureza = '" + this.TxtCodNatureza_Natureza_NotaFiscal.Text.Trim() + "'");
                    this.TxtCodNatureza_Natureza_NotaFiscal.Text = objNatureza.CodNatureza;
                    this.TxtDescNatureza_Natureza_NotaFiscal.Text = objNatureza.DescNatureza;
                }
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[ Editar/Atualizar Especie ]
        protected void LkbEditarEspecie_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodEspecie_Especie_NotaFiscal, typeof(Especie), "Especie.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarEspecie_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.TxtCodEspecie_Especie_NotaFiscal.Text))
                {
                    Especie objEspecie = new Especie();
                    objEspecie.GetBy("CodEspecie = '" + TxtCodEspecie_Especie_NotaFiscal.Text.Trim() + "'");
                    this.TxtCodEspecie_Especie_NotaFiscal.Text = objEspecie.CodEspecie;
                    this.TxtDescEspecie_Especie_NotaFiscal.Text = objEspecie.DescEspecie;
                }
                else
                {
                    PaginaWeb.PreencherControleDropDown(this.Page, DdlIdEspecie_Especie_XML);
                }
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[ Editar/Atualizar Coleta NF ]
        protected void LkbEditaLocalColeta_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado, typeof(Municipio), "Municipio.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarLocalColeta_Click(object sender, EventArgs e)
        {
            try
            {
                preencherMunicipio(this.TxtCodMunicipio_MunicipioColeta_NotaFiscal, this.TxtDescMunicipio_MunicipioColeta_NotaFiscal,
                    this.TxtUF_MunicipioColeta_NotaFiscal);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[ Editar/Atualizar Coleta NF ]
        protected void LkbEditaLocalColetaNF_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodMunicipio_MunicipioColeta_NotaFiscal, typeof(Municipio), "Municipio.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarLocalColetaNF_Click(object sender, EventArgs e)
        {
            try
            {
                preencherMunicipio(TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado, TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado,
                   TxtUF_MunicipioColeta_ConhecimentoOtimizado);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[ Editar/Atualizar Local Entrega NF ]
        protected void LkbEditaEntrega_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodMunicipio_MunicipioEntrega_NotaFiscal, typeof(Municipio), "Municipio.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbEditarNota_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "NotaFiscal", "<script type='text/javascript' language='javascript'>Editar('" + IdNotaFiscal.ToString() + "','../NotaFiscal/NotaFiscal.aspx');</script>", false);
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarEntrega_Click(object sender, EventArgs e)
        {
            try
            {
                preencherMunicipio(TxtCodMunicipio_MunicipioEntrega_NotaFiscal, TxtDescMunicipio_MunicipioEntrega_NotaFiscal,
                    TxtUF_MunicipioEntrega_NotaFiscal);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        protected void LkbAtualizarFilial_Click(object sender, EventArgs e)
        {
            try
            {
                PaginaWeb.PreencherControleDropDown(this.Page, DdlIdFilial_Filial_NotaFiscal);
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbEditarFilial_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(DdlIdFilial_Filial_NotaFiscal, "Filial.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void BtnSalvarNotaFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                salvarNotaFiscal();
                ClassSQLCarga.stParametros objstParam = PopularObjParametros();
                ControleGridView.CarregarNotasDisponiveisBD(this.GvwListaNotasDisponiveis, this.GridNotasFiscaisSelecionadas, this.DdlTipoConsulta.SelectedValue, objstParam, _NumMaxRegistrosConsulta);
                AtualizarInformacoesControlesWeb();
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
                limparNotaFiscal();
            }
            catch (Exception erro)
            {
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 1;
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void BtnSalvarNovoNotaFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                salvarNotaFiscal();
                limparNotaFiscal();
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 1;
                this.SMProgramacao.SetFocus(this.DdlIdFilial_Filial_NotaFiscal.ClientID);
            }
            catch (Exception erro)
            {
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 1;
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void BtnAbortarNotaFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                ClassSQLCarga.stParametros objstParam = PopularObjParametros();
                AtualizarInformacoesControlesWeb();
                limparNotaFiscal();
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
            }
            catch (Exception erro)
            {
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        #region [       Remetente       ]
        protected void LkbAtualizarRemetente_Click(object sender, EventArgs e)
        {
            try
            {
                preencherCliente(this.TxtCodCliente_Remetente_NotaFiscal, this.TxtNome_Remetente_NotaFiscal);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void LkbEditarRemetente_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodCliente_Remetente_NotaFiscal, typeof(Cliente), "Cliente.aspx");
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region [      Destinatario     ]
        protected void LkbAtualizarDestinatario_Click(object sender, EventArgs e)
        {
            try
            {
                preencherCliente(this.TxtCodCliente_Destinatario_NotaFiscal, this.TxtNome_Destinatario_NotaFiscal);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void LkbEditarDestinatario_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodCliente_Destinatario_NotaFiscal, typeof(Cliente), "Cliente.aspx");
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        protected void FormatarPesoVolume(object sender, EventArgs e)
        {
            int idFilial = Convert.ToInt32(this.DdlIdFilial_Filial_NotaFiscal.SelectedValue);
            if (idFilial > 0)
            {
                ControlesWeb.FormatarPesoVolume(Page, idFilial);

                InformacaoPadrao objInformacaoPadrao = new InformacaoPadrao();
                objInformacaoPadrao.GetBy("IdFilial = " + idFilial.ToString());

                this.TxtSerieNF_NotaFiscal.Text = objInformacaoPadrao.SerieCTRC;
                this.TxtCodNatureza_Natureza_NotaFiscal.Text = objInformacaoPadrao.Natureza.CodNatureza;
                this.TxtDescNatureza_Natureza_NotaFiscal.Text = objInformacaoPadrao.Natureza.DescNatureza;

                this.TxtCodEspecie_Especie_NotaFiscal.Text = objInformacaoPadrao.Especie.CodEspecie;
                this.TxtDescEspecie_Especie_NotaFiscal.Text = objInformacaoPadrao.Especie.DescEspecie;

                this.TxtCodCFOP_CFOP_NotaFiscal.Text = objInformacaoPadrao.CFOP.CodCFOP;
                this.TxtDescCFOP_CFOP_NotaFiscal.Text = objInformacaoPadrao.CFOP.DescCFOP;
            }
        }

        private void salvarNotaFiscal()
        {
            try
            {
                NotaFiscal objNotaFiscal = new NotaFiscal();
                objNotaFiscal.GetById(IdNotaFiscal);
                InstanciaClassGeneric.PreencherObjeto(Page.Controls, objNotaFiscal, IdNotaFiscal);
                objNotaFiscal.TotalMercadoria = objNotaFiscal.TotalNF;
                objNotaFiscal.CifFob = '1';
                objNotaFiscal.IdNotaFiscal = IdNotaFiscal;

                if (objNotaFiscal.IdNatureza == 0)
                    throw new Exception("Informe a natureza da nota fiscal");

                if (objNotaFiscal.IdEspecie == 0)
                    throw new Exception("Informe a espécie da nota fiscal");

                PaginaWeb.preencherCamposAuditoria(objNotaFiscal, Page);
                objNotaFiscal.Salvar();

                if (objNotaFiscal.IdNotaFiscal > 0)
                    PaginaWeb.Mensagem(Page, "Nota Fiscal gravada com sucesso", TipoMensagem.Information);
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        private void limparNotaFiscal()
        {
            IdNotaFiscal = 0;
            NotaFiscal objNotaFiscal = new NotaFiscal();
            objNotaFiscal.GetById(0);
            ControlesWeb.PreencheWebControls(objNotaFiscal, Page);
        }

        #endregion

        #region [ Metodos para importacao de XML ]
        protected void BtnListarXML_Click(object sender, EventArgs e)
        {
            if (this.RbtGeracaoCtePorNota.Checked)
            {
                try
                {

                    TextBoxList.Clear();
                    TxtListagemArquivo.Text = "";
                    LblListarXML.Text = "";
                    this.LblMensagemImportacaoXML.Text = "";

                    try
                    {
                        HttpFileCollection hfc = Request.Files;
                        for (int i = 0; i < hfc.Count; i++)
                        {
                            HttpPostedFile hpf = hfc[i];

                            if (!string.IsNullOrEmpty(hpf.FileName))
                            {
                                if (hpf.FileName.ToUpper().EndsWith(".XML"))
                                {
                                    hpf.SaveAs(caminho + System.IO.Path.GetFileName(hpf.FileName));
                                    TxtListagemArquivo.Text += hpf.FileName + Environment.NewLine;
                                    TextBoxList.Add(hpf.FileName);
                                }
                            }
                        }
                    }
                    catch
                    {
                        throw new Exception("Listagem não realizada! Nenhum arquivo XML de NF-e selecionado ou seleção ultrapassou 590 arquivos");
                    }

                    if (TextBoxList.Count == 0)
                        throw new Exception("Listagem não realizada! Nenhum arquivo XML de NF-e selecionado ou seleção ultrapassou 590 arquivos");

                    LblListarXML.Text = TextBoxList.Count.ToString() + " arquivos listados.";
                }
                catch (Exception erro)
                {
                    this.LblMensagemImportacaoXML.ForeColor = Color.Red;
                    this.LblMensagemImportacaoXML.Text = erro.Message;
                }
            }
            else
            {
                try
                {

                    TextBoxList.Clear();
                    TxtListagemArquivoCTeTerceiro.Text = "";
                    LblListarXMLCTeTerceiro.Text = "";
                    this.LblMensagemImportacaoXMLCTeTerceiro.Text = "";

                    try
                    {
                        HttpFileCollection hfc = Request.Files;
                        for (int i = 0; i < hfc.Count; i++)
                        {
                            HttpPostedFile hpf = hfc[i];

                            if (!string.IsNullOrEmpty(hpf.FileName))
                            {
                                if (hpf.FileName.ToUpper().EndsWith(".XML"))
                                {
                                    hpf.SaveAs(caminhoCTETerceiro + System.IO.Path.GetFileName(hpf.FileName));
                                    TxtListagemArquivoCTeTerceiro.Text += hpf.FileName + Environment.NewLine;
                                    TextBoxList.Add(hpf.FileName);
                                }
                            }
                        }
                    }
                    catch
                    {
                        throw new Exception("Listagem não realizada! Nenhum arquivo XML de CT-e selecionado ou seleção ultrapassou 590 arquivos");
                    }

                    if (TextBoxList.Count == 0)
                        throw new Exception("Listagem não realizada! Nenhum arquivo XML de CT-e selecionado ou seleção ultrapassou 590 arquivos");

                    LblListarXMLCTeTerceiro.Text = TextBoxList.Count.ToString() + " arquivos listados.";
                }
                catch (Exception erro)
                {
                    this.LblMensagemImportacaoXMLCTeTerceiro.ForeColor = Color.Red;
                    this.LblMensagemImportacaoXMLCTeTerceiro.Text = erro.Message;
                }
            }
        }
        protected void BtnAbrirImportacaoXML_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RbtGeracaoCtePorNota.Checked)
                {
                    if(mvwPrincipal.ActiveViewIndex==1)
                        this.mvwPrincipal.ActiveViewIndex = 0;
                    else
                        this.mvwPrincipal.ActiveViewIndex = 1;

                    this.mvwGeral.ActiveViewIndex = 0;

                    this.SMProgramacao.SetFocus(this.DdlIdFilial_Filial_NotaFiscal.ClientID);

                    ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
                    objConfiguraModulo.Get();

                    if (objConfiguraModulo.SugerirInformacaoCadastroOtimizadoImportacaoXML == (byte)eSimNao.Sim)
                        determinarInformacaoPadrao();
                }
                else
                {
                    this.mvwPrincipal.ActiveViewIndex = 2;
                    this.mvwGeral.ActiveViewIndex = 0;
                }
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "ImportaCTeTerceiro", "<script type='text/javascript' language='javascript'>open('../ImportaCTeTerceiro/ImportaCTeTerceiro.aspx','ImportaCTeTerceiro')</script>", false);

            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        protected void BtnImportar_Click(object sender, EventArgs e)
        {
            if (RbtGeracaoCtePorNota.Checked)
            {
                try
                {
                    ImportarArquivos();
                }
                catch (Exception erro)
                {
                    //PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
                    this.LblMensagemImportacaoXML.ForeColor = Color.Red;
                    this.LblMensagemImportacaoXML.Text = erro.Message;
                }
            }
            else
            {
                try
                {
                    ImportarArquivosCTeTerceiro();
                }
                catch (Exception erro)
                {
                    this.LblMensagemImportacaoXMLCTeTerceiro.ForeColor = Color.Red;
                    this.LblMensagemImportacaoXMLCTeTerceiro.Text = erro.Message;
                }
            }
        }

        protected void BtnAbortarImportacaoXML_Click(object sender, EventArgs e)
        {
            try
            {
                GridView gvwDisponiveis = this.RbtGeracaoCtePorNota.Checked ? this.GvwListaNotasDisponiveis : this.GvwListaCTeTerceiroDisponiveis;
                GridView gvwSelecionadas = this.RbtGeracaoCtePorNota.Checked ? this.GridNotasFiscaisSelecionadas : this.GvwListaCTeTerceiroSelecionadas;

                TextBoxList.Clear();
                ImportaXMLNFeList.Clear();
                ClassSQLCarga.stParametros objstParam = PopularObjParametros();

                TxtListagemArquivo.Text = "";
                TxtListagemArquivoCTeTerceiro.Text = "";
                LblListarXML.Text = "";
                LblListarXMLCTeTerceiro.Text = "";
                if (this.RbtGeracaoCtePorNota.Checked)
                    ControleGridView.CarregarNotasDisponiveisBD(gvwDisponiveis, gvwSelecionadas, this.DdlTipoConsulta.SelectedValue, objstParam, _NumMaxRegistrosConsulta);
                else
                    this.RbtGeracaoCtePorCte_Click(sender, e);

                AtualizarInformacoesControlesWeb();
                this.TrataVisualGridNotasCTe();
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
            }
            catch (Exception erro)
            {
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
                if (this.RbtGeracaoCtePorNota.Checked)
                {
                    this.LblMensagemImportacaoXML.ForeColor = Color.Red;
                    this.LblMensagemImportacaoXML.Text = erro.Message;
                }
                else
                {
                    this.LblMensagemImportacaoXMLCTeTerceiro.ForeColor = Color.Red;
                    this.LblMensagemImportacaoXMLCTeTerceiro.Text = erro.Message;
                }
                return;
            }
        }


        private void ImportarArquivos()
        {
            this.LblMensagemImportacaoXML.Text = "";
            int _idFilial = retornaItemSelecionado(this.DdlIdFilial_Filial_XML);
            byte _tipoNota = (byte)retornaItemSelecionado(this.DdlTipoNotaXML);
            int _idOperacaoFiscal = 0;
            int _idNatureza = retornaItemSelecionado(this.DdlIdNatureza_Natureza_XML);
            int _idEspecie = retornaItemSelecionado(this.DdlIdEspecie_Especie_XML);
            int _idUltimoUsuario = PaginaWeb.GetIdUsuarioCorrente(Page);
            bool _atualizarDadosCadastrais = this.chkAtualizarDadosCadastrais.Checked;

            #region [ Validações ]
            if (_idFilial == 0)
                throw new Exception("Informe a Filial");

            if (_tipoNota == (byte)0)
                throw new Exception("Informe o Tipo de Nota");

            if (_idNatureza == 0)
                throw new Exception("Informe a Natureza");

            if (_idEspecie == 0)
                throw new Exception("Informe a Espécie");
            #endregion

            ImportaXMLNFeList.Clear();

            #region [ Busca no proprio componente ]
            try
            {
                HttpFileCollection hfc = Request.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFile hpf = hfc[i];

                    if (!string.IsNullOrEmpty(hpf.FileName))
                    {
                        if (hpf.FileName.ToUpper().EndsWith(".XML"))
                        {
                            if (!Directory.Exists(caminho))
                                Directory.CreateDirectory(caminho);

                            hpf.SaveAs(caminho + System.IO.Path.GetFileName(hpf.FileName));
                            ImportaXMLNFeList.Add(caminho + System.IO.Path.GetFileName(hpf.FileName));
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Importação não realizada! Nenhum arquivo XML de NF-e selecionado ou seleção ultrapassou 590 arquivos");
            }
            #endregion

            #region [ Se nao achou no componente pode ter sido listado antes ]
            if (ImportaXMLNFeList.Count == 0)
            {
                for (int i = 0; i < TextBoxList.Count; i++)
                {
                    string linha = (string)TextBoxList[i];
                    ImportaXMLNFeList.Add(caminho + System.IO.Path.GetFileName(linha));
                }
            }
            #endregion

            #region [ Processo de importacao ]
            ArrayList arrayList = ImportaXMLNFeList;

            if (arrayList == null)
                return;

            if (arrayList.Count == 0)
                throw new Exception("Importação não realizada! Nenhum arquivo XML de NF-e selecionado ou seleção ultrapassou 590 arquivos");

            bool todosArqImportados = true;

            for (int i = 0; i < arrayList.Count; i++)
            {
                string path = (string)arrayList[i];

                if (!string.IsNullOrEmpty(path))
                {
                    ImportaXMLNFe objImportaXMLNFe = new ImportaXMLNFe(_idFilial, _tipoNota, 0, _idOperacaoFiscal, _idNatureza, _idEspecie, 0, _idUltimoUsuario, path, _atualizarDadosCadastrais, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    objImportaXMLNFe.ImportarXML();
                    InserirRelatorio(objImportaXMLNFe);
                    //arrayList[i] = "";
                    //ImportaXMLNFeList = arrayList;
                }
            }
            #endregion

            #region [ Deleta arquivos importados ]
            for (int i = 0; i < arrayList.Count; i++)
            {
                string path = (string)arrayList[i];

                if (!string.IsNullOrEmpty(path))
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                arrayList[i] = "";
            }
            #endregion

            #region [ Mostra relatorio final ]
            TextBoxList.Clear();
            TxtListagemArquivo.Text = "";
            LblListarXML.Text = "";

            if (todosArqImportados)
            {
                ImportaXMLNFeList.Clear();

                #region [ Relatorio de Importacao ]
                //relatorio de resultado da importacao
                ScriptManager.RegisterStartupScript(this, typeof(Page), "mensagem", "<script type='text/javascript' language='javascript'>open('../../Relatorio/NotaFiscalEletronica/PrwNotaFiscalEletronica.aspx','PrwNotaFiscalEletronica')</script>", false);
                #endregion
            }
            #endregion
        }


        #region[       Filial      ]
        protected void LkbEditarFilialXML_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(DdlIdFilial_Filial_XML, "Filial.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }
        #endregion

        #region[       Natureza      ]
        protected void atualizarNatureza()
        {
            try
            {
                PaginaWeb.PreencherControleDropDown(this.Page, DdlIdNatureza_Natureza_XML);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void LkbEditarNaturezaXML_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(DdlIdNatureza_Natureza_XML, "Natureza.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }
        #endregion

        #region[       Espécie       ]
        protected void atualizarEspecie()
        {
            try
            {
                PaginaWeb.PreencherControleDropDown(this.Page, DdlIdEspecie_Especie_XML);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void LkbEditarEspecieXML_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(DdlIdEspecie_Especie_XML, "Especie.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }
        #endregion

        private void InserirRelatorio(ImportaXMLNFe objImportaXMLNFe)
        {
            DataSet dsNotaFiscal = (DataSet)Session["dsPopulado"];
            if (dsNotaFiscal == null)
            {
                CriarDataSet();
                dsNotaFiscal = (DataSet)Session["dsPopulado"];
            }

            DataRow dr = dsNotaFiscal.Tables[0].NewRow();

            inserirValorTabela(dr, "Filial", this.DdlIdFilial_Filial_XML.SelectedItem.Text);
            inserirValorTabela(dr, "TipoNota", this.DdlTipoNotaXML.SelectedItem.Text);
            inserirValorTabela(dr, "OperacaoFiscal", "");

            if (this.DdlIdNatureza_Natureza_XML.SelectedIndex > 0)
                inserirValorTabela(dr, "Natureza", this.DdlIdNatureza_Natureza_XML.SelectedItem.Text.Replace("Nova...", ""));
            else
                inserirValorTabela(dr, "Natureza", "");

            if (this.DdlIdEspecie_Especie_XML.SelectedIndex > 0)
                inserirValorTabela(dr, "Especie", this.DdlIdEspecie_Especie_XML.SelectedItem.Text.Replace("Nova...", ""));
            else
                inserirValorTabela(dr, "Especie", "");

            inserirValorTabela(dr, "Arquivo", objImportaXMLNFe.Arquivo);
            inserirValorTabela(dr, "Chave", objImportaXMLNFe.Chave);
            inserirValorTabela(dr, "NumeroNota", objImportaXMLNFe.NumeroNota);
            inserirValorTabela(dr, "Modelo", objImportaXMLNFe.Modelo);
            inserirValorTabela(dr, "Serie", objImportaXMLNFe.Serie);
            inserirValorTabela(dr, "DataEmissao", objImportaXMLNFe.DataEmissao);
            inserirValorTabela(dr, "Emitente", objImportaXMLNFe.NomeFornecedor);
            inserirValorTabela(dr, "Valor", objImportaXMLNFe.ValorNota);
            inserirValorTabela(dr, "Mensagem", objImportaXMLNFe.Status);

            dsNotaFiscal.Tables[0].Rows.Add(dr);
            Session["dsPopulado"] = dsNotaFiscal;
        }

        private void inserirValorTabela(DataRow _dr, string _campo, string _valor)
        {
            //verifica se existe a coluna (se nao existe cria) e insere o valor passado
            bool contemColuna = _dr.Table.Columns.Contains(_campo);

            if (!contemColuna)
                _dr.Table.Columns.Add(_campo);

            _dr[_campo] = _valor;
        }

        private void CriarDataSet()
        {
            if (this.RbtGeracaoCtePorNota.Checked)
            {
                DataSet dsNotaFiscal = new DataSet();
                dsNotaFiscal.Tables.Add("DataSet");
                dsNotaFiscal.Tables[0].Columns.Add("Filial");
                dsNotaFiscal.Tables[0].Columns.Add("TipoNota");
                dsNotaFiscal.Tables[0].Columns.Add("OperacaoFiscal");
                dsNotaFiscal.Tables[0].Columns.Add("Natureza");
                dsNotaFiscal.Tables[0].Columns.Add("Especie");
                dsNotaFiscal.Tables[0].Columns.Add("Arquivo");
                dsNotaFiscal.Tables[0].Columns.Add("Chave");
                dsNotaFiscal.Tables[0].Columns.Add("NumeroNota");
                dsNotaFiscal.Tables[0].Columns.Add("Modelo");
                dsNotaFiscal.Tables[0].Columns.Add("Serie");
                dsNotaFiscal.Tables[0].Columns.Add("DataEmissao");
                dsNotaFiscal.Tables[0].Columns.Add("Emitente");
                dsNotaFiscal.Tables[0].Columns.Add("Valor");
                dsNotaFiscal.Tables[0].Columns.Add("Mensagem");
                Session["dsPopulado"] = null;
                Session["dsPopulado"] = dsNotaFiscal;
            }
            else
            {
                DataSet dsCTeTerceiro = new DataSet();
                dsCTeTerceiro.Tables.Add("DataSet");
                dsCTeTerceiro.Tables[0].Columns.Add("Filial");
                dsCTeTerceiro.Tables[0].Columns.Add("Emitente");
                dsCTeTerceiro.Tables[0].Columns.Add("Arquivo");
                dsCTeTerceiro.Tables[0].Columns.Add("ChaveCTe");
                dsCTeTerceiro.Tables[0].Columns.Add("SerieCTRC");
                dsCTeTerceiro.Tables[0].Columns.Add("NumCTRC");
                dsCTeTerceiro.Tables[0].Columns.Add("DataEmissao");
                dsCTeTerceiro.Tables[0].Columns.Add("Valor");
                dsCTeTerceiro.Tables[0].Columns.Add("Mensagem");
                Session["dsPopulado"] = null;
                Session["dsPopulado"] = dsCTeTerceiro;
            }
        }

        private int retornaItemSelecionado(DropDownList ddlList)
        {
            try
            {
                if (ddlList == null)
                    return 0;

                if (ddlList.SelectedItem == null)
                    return 0;

                return Convert.ToInt32(ddlList.SelectedItem.Value);
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region [ Metodos para consulta de notas ]
        protected void BtnConsultarNotaFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                FiltraPesquisa();
                BtnLimparConsultaNotaFiscal_Click(sender, e);
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void GvwLista_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = ControleGridView.TransformarGridViewInDataTable(this.GvwListaNotasDisponiveis, false);

                #region [ Filtragem ]
                DataView dv = new DataView(dt.Copy());
                dv.Sort = e.SortExpression;

                this.GvwListaNotasDisponiveis.DataSource = dv;
                this.GvwListaNotasDisponiveis.DataBind();
                #endregion
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
                return;
            }
        }

        public ClassSQLCarga.stParametros PopularObjParametros()
        {
            ClassSQLCarga.stParametros objstParametros = new ClassSQLCarga.stParametros();
            try
            {


                objstParametros.Filial = _objTools.FieldInt(this.DdlIdFilial_Filial_NotaFiscalConsulta.SelectedValue);
                objstParametros.Notafiscal = this.TxtNumeroNF_NotaFiscalConsulta.Text.Trim();
                objstParametros.DataEmissao = this.TxtDataEmissaoConsultaInicial.Text;
                objstParametros.CodRemetente = _objTools.FieldInt(this.TxtCodCliente_Remetente_NotaFiscalConsulta.Text.Trim());
                objstParametros.Remetente = this.TxtNome_Remetente_NotaFiscalConsulta.Text.Trim();
                objstParametros.CodDestinatario = _objTools.FieldInt(this.TxtCodCliente_Destinatario_NotaFiscalConsulta.Text.Trim());
                objstParametros.Destinatario = this.TxtNome_Destinatario_NotaFiscalConsulta.Text.Trim();
                objstParametros.LocalColeta = string.IsNullOrEmpty(this.TxtUF_MunicipioColeta_NotaFiscalConsulta.Text) ? "" : (this.TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta.Text.Trim() + "\\" + TxtUF_MunicipioColeta_NotaFiscalConsulta.Text.Trim());
                objstParametros.LocalEntrega = string.IsNullOrEmpty(this.TxtUF_MunicipioEntrega_NotaFiscalConsulta.Text) ? "" : (this.TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta.Text.Trim() + "\\" + TxtUF_MunicipioEntrega_NotaFiscalConsulta.Text.Trim());
                objstParametros.DescNatureza = this.TxtDescNatureza_Natureza_NotaFiscalConsulta.Text.Trim();
                objstParametros.DescEspecie = this.TxtDescEspecie_Especie_NotaFiscalConsulta.Text.Trim();
                objstParametros.ChaveEletronicaNFe = this.TxtChaveEletronicaNFe_NotaFiscalConsulta.Text.Trim();
                objstParametros.CifFob = this.DdlCifFobConsulta.SelectedValue;
                objstParametros.NumeroCarga = TxtNumeroCarga_NotaFiscalConsulta.Text.Trim();
                if (Session["idUsuarioLog"] != null)
                {
                    objstParametros.UsuarioLog = _objTools.FieldInt(Session["idUsuarioLog"]);
                }




            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
            return objstParametros;
        }



        private void FiltraPesquisa()
        {
            try
            {
                if (this.RbtGeracaoCtePorNota.Checked)
                {
                    ClassSQLCarga.stParametros objstParam = PopularObjParametros();
                    ControleGridView.CarregarNotasDisponiveisBD(this.GvwListaNotasDisponiveis, this.GridNotasFiscaisSelecionadas, this.DdlTipoConsulta.SelectedValue, objstParam, _NumMaxRegistrosConsulta);
                    DataTable dt = ControleGridView.TransformarGridViewInDataTable(this.GvwListaNotasDisponiveis, false, this.RbtGeracaoCtePorNota.Checked);
                    dt = ControleGridView.ValidarDocumentosDisponiveisSelecionados(dt, GridNotasFiscaisSelecionadas,this.RbtGeracaoCtePorNota.Checked, false);
                    DataView dv = new DataView(dt.Copy());

                    this.GvwListaNotasDisponiveis.DataSource = dv;
                    if (dv.Table.Rows.Count == _NumMaxRegistrosConsulta && _NumMaxRegistrosConsulta > 0)
                    {
                        PaginaWeb.Mensagem(Page, "Foram carregados apenas os " + _NumMaxRegistrosConsulta + " registros iniciais!", TipoMensagem.Information);
                    }
                    this.GvwListaNotasDisponiveis.DataBind();
                    AtualizarInformacoesControlesWeb();
                }
                else
                {

                    ClassSQLCarga.stParametrosCTeTerceiro objstParametro = PopularObjParametrosCTeTerceiro();
                    ControleGridView.CarregarConhecimentoTerceiroDisponiveisBD(this.GvwListaCTeTerceiroDisponiveis, this.GvwListaCTeTerceiroSelecionadas, this.DdlTipoConsultaCteTerceiro.SelectedValue, objstParametro, _NumMaxRegistrosConsulta);
                    DataTable dt = ControleGridView.TransformarCTeTerceiroGridViewInDataTable(this.GvwListaCTeTerceiroDisponiveis, false, this.RbtGeracaoCtePorCte.Checked);
                    dt = ControleGridView.ValidarDocumentosDisponiveisSelecionados(dt, GvwListaCTeTerceiroSelecionadas,false, this.RbtGeracaoCtePorCte.Checked);
                    DataView dv = new DataView(dt.Copy());
                    this.GvwListaCTeTerceiroDisponiveis.DataSource = dv;

                    if (dv.Table.Rows.Count == _NumMaxRegistrosConsulta && _NumMaxRegistrosConsulta > 0)
                    {
                        PaginaWeb.Mensagem(Page, "Foram carregados apenas os " + _NumMaxRegistrosConsulta + " registros iniciais!", TipoMensagem.Information);
                    }
                    this.GvwListaCTeTerceiroDisponiveis.DataBind();

                    AtualizarInformacoesControlesWeb();
                }
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        #region[ Editar/Atualizar Coleta NF ]
        protected void LkbEditaLocalColetaConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodMunicipio_MunicipioColeta_NotaFiscal, typeof(Municipio), "Municipio.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarLocalColetaConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                preencherMunicipio(TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado, TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado,
                    TxtUF_MunicipioColeta_ConhecimentoOtimizado);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[ Editar/Atualizar Local Entrega NF ]
        protected void LkbEditaEntregaConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodMunicipio_MunicipioEntrega_NotaFiscal, typeof(Municipio), "Municipio.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarEntregaConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                preencherMunicipio(TxtCodMunicipio_MunicipioEntrega_NotaFiscal, TxtDescMunicipio_MunicipioEntrega_NotaFiscal,
                    TxtUF_MunicipioEntrega_NotaFiscal);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[ Editar/Atualizar Natureza ]
        protected void LkbEditarNaturezaConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodNatureza_Natureza_NotaFiscal, typeof(Natureza), "Natureza.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarNaturezaConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                Natureza objNatureza = new Natureza();
                objNatureza.GetBy("CodNatureza = '" + this.TxtCodNatureza_Natureza_NotaFiscal.Text.Trim() + "'");
                this.TxtCodNatureza_Natureza_NotaFiscalConsulta.Text = objNatureza.CodNatureza;
                this.TxtDescNatureza_Natureza_NotaFiscalConsulta.Text = objNatureza.DescNatureza;
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region[ Editar/Atualizar Especie ]
        protected void LkbEditarEspecieConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.LinkButtonEditar(this.TxtCodEspecie_Especie_NotaFiscal, typeof(Especie), "Especie.aspx");
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected void LkbAtualizarEspecieConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                Especie objEspecie = new Especie();
                objEspecie.GetBy("CodEspecie = '" + TxtCodEspecie_Especie_NotaFiscal.Text.Trim() + "'");
                this.TxtCodEspecie_Especie_NotaFiscalConsulta.Text = objEspecie.CodEspecie;
                this.TxtDescEspecie_Especie_NotaFiscalConsulta.Text = objEspecie.DescEspecie;
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        protected void BtnAbrirConsultaNotasFiscais_Click(object sender, EventArgs e)
        {
            try
            {
                this.mvwPrincipal.ActiveViewIndex = 0;

                if (!this.RbtGeracaoCtePorNota.Checked)
                {
                    this.mvwGeral.ActiveViewIndex = 3;
                    this.SMProgramacao.SetFocus(this.DdlIdFilial_Filial_CteTerceiroConsulta.ClientID);
                }
                else
                {
                    this.mvwGeral.ActiveViewIndex = 2;
                    this.SMProgramacao.SetFocus(this.DdlIdFilial_Filial_NotaFiscalConsulta.ClientID);
                }


            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void BtnAbortarConsultaNF_Click(object sender, EventArgs e)
        {
            try
            {
                BtnLimparConsultaNotaFiscal_Click(sender, e);
                this.TrataVisualGridNotasCTe();
                this.FiltraPesquisa();
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
            }
            catch (Exception erro)
            {
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
                return;
            }
        }

        protected void BtnLimparConsultaNotaFiscal_Click(object sender, EventArgs e)
        {
            try
            {
                this.DdlTipoConsulta.SelectedIndex = 0;
                this.DdlIdFilial_Filial_NotaFiscalConsulta.SelectedIndex = 0;
                this.TxtNumeroNF_NotaFiscalConsulta.Text = "";
                this.TxtDataEmissaoConsultaInicial.Text = "";

                this.TxtCodCliente_Destinatario_NotaFiscalConsulta.Text = "";
                this.TxtNome_Destinatario_NotaFiscalConsulta.Text = "";
                this.TxtCnpjCpf_Destinatario_NotaFiscalConsulta.Text = "";

                this.TxtCodCliente_Remetente_NotaFiscalConsulta.Text = "";
                this.TxtNome_Remetente_NotaFiscalConsulta.Text = "";
                this.TxtCnpjCpf_Remetente_NotaFiscalConsulta.Text = "";

                this.TxtCodMunicipio_MunicipioColeta_NotaFiscalConsulta.Text = "";
                this.TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta.Text = "";
                this.TxtUF_MunicipioColeta_NotaFiscalConsulta.Text = "";

                this.TxtCodMunicipio_MunicipioEntrega_NotaFiscalConsulta.Text = "";
                this.TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta.Text = "";
                this.TxtUF_MunicipioEntrega_NotaFiscalConsulta.Text = "";

                this.TxtCodNatureza_Natureza_NotaFiscalConsulta.Text = "";
                this.TxtDescNatureza_Natureza_NotaFiscalConsulta.Text = "";

                this.TxtCodEspecie_Especie_NotaFiscalConsulta.Text = "";
                this.TxtDescEspecie_Especie_NotaFiscalConsulta.Text = "";

                this.TxtChaveEletronicaNFe_NotaFiscalConsulta.Text = "";
                this.TxtNumeroCarga_NotaFiscalConsulta.Text = "";
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void BtnRetirarInserirNF_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.RedirecionarSimples(Page, "InutilizacaoNFCadastroOtimizado.aspx", "Origem=ConhecimentoOtimizado.aspx");
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        #endregion

        #region [ Atualizar Municipio Centralizado ]
        protected void preencherMunicipio(TextBox txtCodigo, TextBox txtDescricao, TextBox txtUF)
        {
            if (txtDescricao != null)
                txtDescricao.Text = "";

            if (txtUF != null)
                txtUF.Text = "";

            if (!String.IsNullOrEmpty(txtCodigo.Text))
            {
                Municipio objMunicipio = new Municipio();
                objMunicipio.GetByCodigo(txtCodigo.Text.Trim());
                txtCodigo.Text = objMunicipio.CodMunicipio.ToString();

                if (txtDescricao != null)
                    txtDescricao.Text = objMunicipio.DescMunicipio;

                if (txtUF != null)
                    txtUF.Text = objMunicipio.UF;
            }
        }
        #endregion

        #region [ Atualizar Cliente Centralizado ]
        protected void preencherCliente(TextBox txtCodigo, TextBox txtNome)
        {
            if (txtNome != null)
                txtNome.Text = "";

            if (!String.IsNullOrEmpty(txtCodigo.Text))
            {
                Cliente objCliente = new Cliente();
                objCliente.GetByCodigo(txtCodigo.Text.Trim());
                txtCodigo.Text = objCliente.CodCliente.ToString();

                if (txtNome != null)
                    txtNome.Text = objCliente.Nome;
            }
        }
        #endregion

        #region [ Atualizar Veiculo Centralizado ]
        protected void preencherVeiculo(TextBox txtCodigo, TextBox txtPlaca)
        {
            if (txtPlaca != null)
                txtPlaca.Text = "";

            if (!String.IsNullOrEmpty(txtCodigo.Text))
            {
                Veiculo objVeiculo = new Veiculo();
                objVeiculo.GetBy("CodVeiculo = '" + txtCodigo.Text.Trim() + "'");
                txtCodigo.Text = objVeiculo.CodVeiculo;

                if (txtPlaca != null)
                    txtPlaca.Text = objVeiculo.Placa;
            }
        }
        #endregion


        private DataTable getDataTableTabelaFrete()
        {
            try
            {
                DataTable tbTabelaFrete = new DataTable();
                tbTabelaFrete.Columns.Add("IdTabelaFrete", typeof(string));
                tbTabelaFrete.Columns.Add("DescTipoTabela", typeof(string));
                tbTabelaFrete.Columns.Add("DescTipoTransporte", typeof(string));
                tbTabelaFrete.Columns.Add("DescCliente", typeof(string));
                tbTabelaFrete.Columns.Add("DescLocalOrigem", typeof(string));
                tbTabelaFrete.Columns.Add("DescLocalDestino", typeof(string));
                tbTabelaFrete.Columns.Add("DescEspecie", typeof(string));
                tbTabelaFrete.Columns.Add("DescNatureza", typeof(string));
                tbTabelaFrete.Columns.Add("Validade", typeof(string));

                return tbTabelaFrete;
            }
            catch
            {
                return null;
            }
        }

        protected void BtnTabelaFrete_Click(object sender, EventArgs e)
        {
            try
            {
                this.LblMensagemModalTabelaFrete.Text = "";
                listarTabelaFreteDisponivel();
                this.MpeTabelaFrete.Show();
            }
            catch (Exception erro)
            {
                this.LblMensagem.ForeColor = Color.Red;
                this.LblMensagem.Text = erro.Message;
            }
        }

        protected void SelecionarTabelaFrete_Click(object sender, CommandEventArgs e)
        {
            try
            {
                Label lblIdTabelaFrete = new Label();
                CheckBox check = new CheckBox();
                List<int> listaIDsTabelaFrete = new List<int>();
                DataTable tbTabelaFrete = getDataTableTabelaFrete();

                this.LblMensagemModalTabelaFrete.Text = "";


                foreach (GridViewRow gvwTabelaFrete in this.GvwListaTabelaFrete.Rows)
                {
                    lblIdTabelaFrete = (Label)gvwTabelaFrete.FindControl("IdTabelaFrete");
                    check = (CheckBox)gvwTabelaFrete.FindControl("chkselect");
                    if (check == null || lblIdTabelaFrete == null)
                        continue;

                    if (check.Checked)
                    {
                        int _IdTabelaFrete = _objTools.FieldInt(lblIdTabelaFrete.Text);
                        if (_IdTabelaFrete > 0)
                        {
                            Label lblTipoTabela_TabelaFrete = (Label)gvwTabelaFrete.FindControl("DescTipoTabela");
                            Label lblTipoTransporte_TabelaFrete = (Label)gvwTabelaFrete.FindControl("DescTipoTransporte");
                            Label lblCliente_TabelaFrete = (Label)gvwTabelaFrete.FindControl("DescCliente");
                            Label lblLocalOrigem_TabelaFrete = (Label)gvwTabelaFrete.FindControl("DescLocalOrigem");
                            Label lblLocalDestino_TabelaFrete = (Label)gvwTabelaFrete.FindControl("DescLocalDestino");

                            Label lblEspecie_TabelaFrete = (Label)gvwTabelaFrete.FindControl("DescEspecie");
                            Label lblNatureza_TabelaFrete = (Label)gvwTabelaFrete.FindControl("DescNatureza");
                            Label lblValidade_TabelaFrete = (Label)gvwTabelaFrete.FindControl("Validade");

                            if (lblTipoTabela_TabelaFrete == null || lblTipoTransporte_TabelaFrete == null
                                || lblCliente_TabelaFrete == null || lblLocalOrigem_TabelaFrete == null || lblLocalDestino_TabelaFrete == null || lblEspecie_TabelaFrete == null
                                || lblNatureza_TabelaFrete == null || lblValidade_TabelaFrete == null)
                                continue;

                            DataRow newRow = tbTabelaFrete.NewRow();
                            newRow["IdTabelaFrete"] = _IdTabelaFrete.ToString();
                            newRow["DescTipoTabela"] = _objTools.FieldString(lblTipoTabela_TabelaFrete.Text).Trim();
                            newRow["DescTipoTransporte"] = _objTools.FieldString(lblTipoTransporte_TabelaFrete.Text).Trim();
                            newRow["DescCliente"] = _objTools.FieldString(lblCliente_TabelaFrete.Text).Trim().Trim();
                            newRow["DescLocalOrigem"] = _objTools.FieldString(lblLocalOrigem_TabelaFrete.Text).Trim();
                            newRow["DescLocalDestino"] = _objTools.FieldString(lblLocalDestino_TabelaFrete.Text).Trim();
                            newRow["DescEspecie"] = _objTools.FieldString(lblEspecie_TabelaFrete.Text).Trim();
                            newRow["DescNatureza"] = _objTools.FieldString(lblNatureza_TabelaFrete.Text).Trim();
                            newRow["Validade"] = _objTools.FieldString(lblValidade_TabelaFrete.Text).Trim();
                            tbTabelaFrete.Rows.Add(newRow);

                            listaIDsTabelaFrete.Add(_IdTabelaFrete);
                        }
                    }
                }

                if (listaIDsTabelaFrete.Count == 0)
                {
                    this.LblMensagemModalTabelaFrete.Text = "Favor selecionar ao menos uma Tabela de Frete!";
                    this.LblMensagemModalTabelaFrete.ForeColor = Color.Red;
                    return;
                }
                else if (listaIDsTabelaFrete.Count > 1)
                {
                    this.LblMensagemModalTabelaFrete.Text = "Favor selecionar somente uma Tabela de Frete!";
                    this.LblMensagemModalTabelaFrete.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    if (this.DataTableTabelaFrete != null)
                    {
                        if (this.DataTableTabelaFrete.Rows.Count > 0)
                        {
                            this.LblMensagemModalTabelaFrete.Text = "Já existe Tabela de Frete vinculada!";
                            this.LblMensagemModalTabelaFrete.ForeColor = Color.Red;
                            return;
                        }
                    }

                    // insere o registro selecionado
                    this.DataTableTabelaFrete = tbTabelaFrete;
                }

                atualizarListaTabelaFreteDisponivel(listaIDsTabelaFrete[0]);
                this.GvwTabelaFreteSelecionada.DataSource = this.DataTableTabelaFrete;
                this.GvwTabelaFreteSelecionada.DataBind();
            }
            catch (Exception erro)
            {
                this.LblMensagemModalTabelaFrete.Text = erro.Message;
                this.LblMensagemModalTabelaFrete.ForeColor = Color.Red;
                this.MpeTabelaFrete.Show();
            }
            finally
            {
                this.MpeTabelaFrete.Show();
            }
        }

        protected void RemoverTabelaFrete_Click(object sender, CommandEventArgs e)
        {
            try
            {
                Label lblIdTabelaFrete = new Label();
                CheckBox check = new CheckBox();
                bool selected = false;

                this.LblMensagemModalTabelaFrete.Text = "";
                foreach (GridViewRow gvwTabelaFrete in this.GvwTabelaFreteSelecionada.Rows)
                {
                    lblIdTabelaFrete = (Label)gvwTabelaFrete.FindControl("IdTabelaFrete");
                    check = (CheckBox)gvwTabelaFrete.FindControl("chkselect");
                    if (check == null || lblIdTabelaFrete == null)
                        continue;

                    int _IdTabelaFrete = _objTools.FieldInt(lblIdTabelaFrete.Text);
                    if (check.Checked && _IdTabelaFrete > 0)
                    {
                        selected = true;
                        this.DataTableTabelaFrete = new DataTable();
                        GvwTabelaFreteSelecionada.DataSource = null;
                        GvwTabelaFreteSelecionada.DataBind();
                    }
                }

                if (!selected)
                {
                    this.LblMensagemModalTabelaFrete.Text = "Favor selecionar ao menos uma Tabela de Frete!";
                    this.LblMensagemModalTabelaFrete.ForeColor = Color.Red;
                    this.MpeTabelaFrete.Show();
                    return;
                }

                listarTabelaFreteDisponivel();
                this.MpeTabelaFrete.Show();
            }
            catch (Exception erro)
            {
                this.LblMensagemModalTabelaFrete.Text = erro.Message;
                this.LblMensagemModalTabelaFrete.ForeColor = Color.Red;
                this.MpeTabelaFrete.Show();
            }
        }

        private void atualizarListaTabelaFreteDisponivel(int _IdTabelaFreteSelecionada)
        {
            try
            {
                List<string> listaCnpjRemetente = getListaCnpjRemetente();
                int idTipoTransporte = getIdTipoTransporte();

                this.GvwListaTabelaFrete.DataSource = objConhecimentoOtimizado.ListarTabelaFreteDisponivel(listaCnpjRemetente, idTipoTransporte, _IdTabelaFreteSelecionada);
                this.GvwListaTabelaFrete.DataBind();
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        private int getIdTabelaFreteRateamento()
        {
            try
            {
                if (this.DataTableTabelaFrete == null)
                    return 0;

                if (this.DataTableTabelaFrete.Rows.Count > 0)
                    return _objTools.FieldInt(this.DataTableTabelaFrete.Rows[0]["IdTabelaFrete"]);

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private int getIdTipoTransporte()
        {
            try
            {
                int idTipoTransporte = _objTools.FieldInt(DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.SelectedValue);
                return idTipoTransporte;
            }
            catch
            {
                return 0;
            }
        }

        private List<string> getListaCnpjRemetente()
        {
            try
            {
                DataTable tbNotas = ControleGridView.TransformarGridViewInDataTable(GridNotasFiscaisSelecionadas, false);
                if (tbNotas == null)
                    return new List<string>();

                List<string> listaCnpjRemetente = new List<string>();
                foreach (DataRow rowNota in tbNotas.Rows)
                {
                    string _cnpjRemetente = _objTools.FieldString(rowNota["RemetenteCNPJCPF"]).Trim();
                    if (_objTools.IsNullOrEmpty(_cnpjRemetente))
                        continue;

                    if (!listaCnpjRemetente.Contains(_cnpjRemetente))
                        listaCnpjRemetente.Add(_cnpjRemetente);
                }

                return listaCnpjRemetente;
            }
            catch
            {
                return new List<string>();
            }
        }

        protected void ListarRegistrosTabelaFrete_Click(object sender, EventArgs e)
        {
            try
            {
                this.LblMensagemModalTabelaFrete.Text = "";
                listarTabelaFreteDisponivel();
                this.MpeTabelaFrete.Show();
            }
            catch
            {
            }
        }

        private void listarTabelaFreteDisponivel()
        {
            try
            {
                if (getIdTipoTransporte() <= 0)
                    throw new Exception("Favor informar o Tipo de Transporte!");

                int _idTabelaFreteSelecionada = getIdTabelaFreteRateamento();
                atualizarListaTabelaFreteDisponivel(_idTabelaFreteSelecionada);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        private void ExibirMensagemTela(string mensagem, Color objColor)
        {
            try
            {
                mensagem = _objTools.FieldString(mensagem).Trim();
                if (_objTools.IsNullOrEmpty(mensagem))
                    return;

                this.LblMensagem.Text = mensagem;
                this.LblMensagem.ForeColor = objColor;
            }
            catch
            {
            }
        }

        private void controlarVisibilidadeLinkTabelaFrete()
        {
            try
            {
                var objConfiguraModulo = new ConfiguraModulo();
                objConfiguraModulo.Get();
                bool validarRemetenteRaizCnpj =
                (objConfiguraModulo.NoCadastroOtimizadoValidarRemetenteNotasPorRaizCnpj == (byte)eSimNao.Sim);

                LkbTabelaFrete.Enabled = validarRemetenteRaizCnpj;
                LkbTabelaFrete.Visible = validarRemetenteRaizCnpj;
            }
            catch
            {
            }
        }

        private void computarTotalPalletsNotaNaturezaPaletizada()
        {
            try
            {
                objConfiguraModulo.Get();
                NotaFiscal objNotaFiscalRemetente = new NotaFiscal();
                objNotaFiscalRemetente.GetById(IdNotaFiscalRemetente);
                Cliente objRemetente = new Cliente();
                objRemetente.GetById(objNotaFiscalRemetente.IdRemetente);
                bool ConsiderarNotaFiscaldePalletnoCalculoDoFrete = (objRemetente.ConsiderarNotaFiscalDePalletNoCalculoDoFrete == (byte)eSimNao.Sim);
                bool permitirDefinicaoOperacao =
                (objConfiguraModulo.PermitirDefinicaoOperacaoTransporteCadastroOtimizadoCTe == (byte)eSimNao.Sim);
                if (!permitirDefinicaoOperacao && !ConsiderarNotaFiscaldePalletnoCalculoDoFrete)
                    return;

                int operacao = _objTools.FieldInt(DdlTipoOperacaoTransporte_ConhecimentoOtimizado.SelectedValue);
                if (operacao == (byte)eTipoOperacaoTransporte.SemPallet)
                    return;

                int _IdNotaFiscal = 0;
                double volumeTotal = 0.0;
                NotaFiscal objNotaFiscal = new NotaFiscal();
                foreach (GridViewRow rowNotaFiscal in this.GridNotasFiscaisSelecionadas.Rows)
                {
                    if (rowNotaFiscal == null)
                        continue;

                    Label lblIdNotaFiscal = (Label)rowNotaFiscal.FindControl("IdNotaFiscal");
                    if (lblIdNotaFiscal == null)
                        continue;

                    _IdNotaFiscal = _objTools.FieldInt(lblIdNotaFiscal.Text);
                    objNotaFiscal = new NotaFiscal();
                    objNotaFiscal.GetById(_IdNotaFiscal);

                    if (objNotaFiscal.IdNotaFiscal > 0)
                    {
                        if (objNotaFiscal.Natureza.TipoCarga == 4)
                            volumeTotal += objNotaFiscal.VolumeTotal;
                    }
                }

                if (volumeTotal > 0)
                {
                    int totalPallets = (int)volumeTotal;
                    this.TxtTotalPallets_ConhecimentoOtimizado.Text = _objTools.FieldString(totalPallets);
                }
            }
            catch
            {
            }
        }

        private void setInfoFreteGeralM03()
        {
            try
            {
                var objFreteGeralM03 = new FreteGeralM03();
                objFreteGeralM03.GetBy(" 1 = 1");

                if (objFreteGeralM03.IdFreteGeralM03 > 0)
                    this.HIDPossuiFreteCargaGeralM03.Value = "1";
            }
            catch
            {
            }
        }

        private void tratarTipoOperacao()
        {
            try
            {
                string _codPagador = _objTools.FieldString(TxtCodCliente_Pagador_ConhecimentoOtimizado.Text).Trim();
                string _tipoTransporte =
                _objTools.FieldString(DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.SelectedValue).Trim();
                if (_objTools.IsNullOrEmpty(_codPagador) || _objTools.IsNullOrEmpty(_tipoTransporte))
                    return;

                var objPagador = new Cliente();
                objPagador.GetByCodigo(_codPagador);

                var objTipoTransporte = new TipoTransporte();
                objTipoTransporte.GetById(_objTools.FieldInt(_tipoTransporte));

                if (objPagador.IdCliente <= 0 || objTipoTransporte.IdTipoTransporte <= 0)
                    return;
                if (objTipoTransporte.ModeloTabelaFrete != (byte)eModeloTabelaFrete.CargaGeral_Modelo03)
                    return;

                var _objFreteGeralM03 = new FreteGeralM03();
                _objFreteGeralM03 = _objFreteGeralM03.BuscarFreteGeralM03PorPagadorTipoTransporte(objTipoTransporte.IdTipoTransporte, objPagador.IdCliente);

                bool permitirEscolhaTipoOperacao = false;
                if (_objFreteGeralM03.IdFreteGeralM03 > 0)
                {
                    permitirEscolhaTipoOperacao =
                    (_objFreteGeralM03.PermitirEscolhaManualTipoOperacaoNoCadastroOtimizado == (byte)eSimNao.Sim);
                    DdlTipoOperacaoTransporte_ConhecimentoOtimizado.Enabled = permitirEscolhaTipoOperacao;

                    if (!permitirEscolhaTipoOperacao)
                    {
                        var _objFreteGeralM03Operacao = new FreteGeralM03Operacao();
                        _objFreteGeralM03Operacao.GetBy($@" IdFreteGeralM03 = {_objFreteGeralM03.IdFreteGeralM03}");

                        if (_objFreteGeralM03Operacao.IdFreteGeralM03Operacao > 0)
                        {
                            if (DdlTipoOperacaoTransporte_ConhecimentoOtimizado.Items.FindByValue(_objFreteGeralM03Operacao.TipoOperacaoTransporte.ToString()) != null)
                            {
                                DdlTipoOperacaoTransporte_ConhecimentoOtimizado.SelectedValue =
                                    _objFreteGeralM03Operacao.TipoOperacaoTransporte.ToString();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        protected void BtnImportarXMLChave_Click(object sender, EventArgs e)
        {
            try
            {
                ControlesWeb.RedirecionarSimples(Page, "PedidoNFeXML.aspx", "Id=0&Origem=ConhecimentoOtimizado.aspx");
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }
        private void determinarInformacaoPadrao()
        {
            ClassSQLBasico objClassSQLBasico = new ClassSQLBasico();
            DataSet ds = objClassSQLBasico.ListaFilial();

            int idfilial = objClassSQLBasico.Tools.ConvertToInt32(ds.Tables[0].Rows[0]["IdFilial"]);
            string tiponota = "1";

            if (idfilial > 0)
            {
                InformacaoPadrao objInformacaoPadrao = new InformacaoPadrao();
                objInformacaoPadrao.GetByFilial(idfilial);

                this.DdlIdFilial_Filial_XML.SelectedValue = idfilial.ToString();
                this.DdlIdNatureza_Natureza_XML.SelectedValue = objInformacaoPadrao.IdNatureza.ToString();
                this.DdlIdEspecie_Especie_XML.SelectedValue = objInformacaoPadrao.IdEspecie.ToString();
                this.DdlTipoNotaXML.SelectedValue = tiponota;
            }
        }
        private void sugerirRotaCTeCadastroOtimizado()
        {
            try
            {
                NotaFiscal objNotaFiscal = new NotaFiscal();
                RotaCTe objRotaCTe = new RotaCTe();

                GridViewRow gvr = this.GridNotasFiscaisSelecionadas.Rows[0];
                int idNotaFiscal = Convert.ToInt32(((Label)gvr.FindControl("IdNotaFiscal")).Text);
                objNotaFiscal.GetById(idNotaFiscal);

                if (objNotaFiscal.IdNotaFiscal > 0)
                    this.DdlIdRotaCTe_RotaCTe_ConhecimentoOtimizado.SelectedValue = objRotaCTe.SugerirRotaCTe(objNotaFiscal).ToString();

                this.sugerirTomadorRotaCTeCadastroOtimizado(null, null);
            }
            catch
            {
                this.DdlIdRotaCTe_RotaCTe_ConhecimentoOtimizado.SelectedValue = "0";
                return;
            }
        }
        protected void sugerirTomadorRotaCTeCadastroOtimizado(object sender, EventArgs e)
        {
            RotaCTe objRotaCTe = new RotaCTe();
            Cliente objPagador = new Cliente();

            if (objRotaCTe.GetById(objRotaCTe.Tools.FieldInt(this.DdlIdRotaCTe_RotaCTe_ConhecimentoOtimizado.SelectedValue)) && objRotaCTe.idPagador > 0)
            {
                if (objPagador.GetById(objRotaCTe.idPagador))
                {
                    this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objPagador.CodCliente.ToString();
                    this.TxtNome_Pagador_ConhecimentoOtimizado.Text = objPagador.RazaoSocial.ToString();
                    this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objPagador.CnpjCpf.ToString();
                }
            }
        }

        protected void tratarCamposPallet()
        {
            if (DdlTipoOperacaoTransporte_ConhecimentoOtimizado.SelectedValue == ((byte)eTipoOperacaoTransporte.SemPallet).ToString())
            {
                TxtTotalPallets_ConhecimentoOtimizado.Text = "0";
            }

            TxtTotalPallets_ConhecimentoOtimizado.Enabled = DdlTipoOperacaoTransporte_ConhecimentoOtimizado.SelectedValue != ((byte)eTipoOperacaoTransporte.SemPallet).ToString();
            TxtTotalPallets_ConhecimentoOtimizado.CssClass = TxtTotalPallets_ConhecimentoOtimizado.Enabled ? "ctxtObr" : "ctxt";
        }

        protected void LkbComporPesoCubado_Click(object sender, EventArgs e)
        {
            this.MpeComposicaoCubagem.Show();
        }

        protected void BtnConfirmarComposicao_Click(object sender, EventArgs e)
        {
            this.MpeComposicaoCubagem.Hide();
        }

        protected void BtnComporPesoCubadoTotal_Click(object sender, ImageClickEventArgs e)
        {
            this.MpeComposicaoCubagem.Show();
        }

        private void AtualizarVolumeTotal()
        {
            double volume = 0;
            foreach (GridViewRow gvrNota in this.GridNotasFiscaisSelecionadas.Rows)
            {
                Label lblVolume = (Label)gvrNota.FindControl("Volume");
                volume += this._objTools.ConvertToDouble(lblVolume.Text.Trim());
            }
            this.HidVolumeTotal.Value = volume.ToString();
        }

        private void aplicaMascaraPeso()
        {
            int idFilial = this._objTools.ConvertToInt32(((Label)this.GridNotasFiscaisSelecionadas.Rows[0].FindControl("IdFilial")).Text.Trim());
            ControlesWeb.FormataPesoControle(this.TxPTotalPesoCubadoNF_ConhecimentoOtimizado, idFilial);
            PaginaWeb.RegisterStartupScript(this.Page);
        }

        private void AtualizarControlesNFsRelacionadas()
        {
            double volume = 0;
            Filial objFilial = new Filial();

            if (GridNotasFiscaisSelecionadas.Rows.Count != 0)
            {
                Label idFilial = (Label)GridNotasFiscaisSelecionadas.Rows[0].FindControl("IdFilial");
                objFilial.GetById(Convert.ToInt32(idFilial.Text));
            }

            string remetente = "";
            string destinatario = "";
            string localColeta = "";
            string localEntrega = "";
            bool TemLocaisDestintos = false; ;
            bool camposDistintos = false;

            foreach (GridViewRow gvrNota in this.GridNotasFiscaisSelecionadas.Rows)
            {

                Label lblVolume = (Label)gvrNota.FindControl("Volume");
                volume += this._objTools.ConvertToDouble(lblVolume.Text.Trim());

                Label LblFilial = (Label)gvrNota.FindControl("Filial");
                Label LblRemetenteCNPJCPF = (Label)gvrNota.FindControl("RemetenteCNPJCPF");
                Label LblDestinatarioCnpjCpf = (Label)gvrNota.FindControl("DestinatarioCnpjCpf");
                Label LblIdLocalColeta = (Label)gvrNota.FindControl("IdLocalColeta");
                Label LblIdEntrega = (Label)gvrNota.FindControl("IdLocalEntrega");

                if (remetente != "" && !camposDistintos && (remetente != LblRemetenteCNPJCPF.Text || destinatario != LblDestinatarioCnpjCpf.Text || localColeta != LblIdLocalColeta.Text || localEntrega != LblIdEntrega.Text))
                {
                    if (objFilial.NaEmissaoCTePermitirVincularNFeDeLocalEntregaDistinto != (byte)eSimNao.Sim)
                    {
                        TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado.Text = "";
                        TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado.Text = "";
                        TxtUF_MunicipioColeta_ConhecimentoOtimizado.Text = "";

                        TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = "";
                        TxtNome_Pagador_ConhecimentoOtimizado.Text = "";
                        TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = "";

                        camposDistintos = true;
                    }
                    else
                    {
                        TemLocaisDestintos = true;
                    }
                }

                remetente = LblRemetenteCNPJCPF.Text;
                destinatario = LblDestinatarioCnpjCpf.Text;
                localColeta = LblIdLocalColeta.Text;
                localEntrega = LblIdEntrega.Text;
            }
            if (GridNotasFiscaisSelecionadas.Rows.Count != 0)
            {
                if (objFilial.NaEmissaoCTePermitirVincularNFeDeLocalEntregaDistinto == (byte)eSimNao.Sim && TemLocaisDestintos)
                {
                    Label LblRemetenteCNPJCPF = (Label)this.GridNotasFiscaisSelecionadas.Rows[0].FindControl("RemetenteCNPJCPF");
                    PaginaWeb.Mensagem(Page, "Notas Fiscais com Local de Entrega/Coleta ou Destinatários distintos", TipoMensagem.Warning);

                }
            }

            this.HidVolumeTotal.Value = volume.ToString();

            var objConfiguraModulo = new ConfiguraModulo();
            objConfiguraModulo.Get();
            bool manterLocalColetaHabilitados = (objConfiguraModulo.ManterLocalColetaHabilitadoNoCadastroOtimizado == (byte)eSimNao.Sim);

            TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado.Enabled =
                (manterLocalColetaHabilitados) ? true : (!camposDistintos);
            TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado.Enabled =
                (manterLocalColetaHabilitados) ? true : (!camposDistintos);
            TxtUF_MunicipioColeta_ConhecimentoOtimizado.Enabled =
                (manterLocalColetaHabilitados) ? true : (!camposDistintos);
            LkbAtualizaColeta.Enabled = (manterLocalColetaHabilitados) ? true : (!camposDistintos);
            LkbEditarColeta.Enabled = (manterLocalColetaHabilitados) ? true : (!camposDistintos);

            TxtCodCliente_Pagador_ConhecimentoOtimizado.Enabled = !camposDistintos;
            TxtNome_Pagador_ConhecimentoOtimizado.Enabled = !camposDistintos;
            TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Enabled = !camposDistintos;
            LkbAtualizarPagador.Enabled = !camposDistintos;
            LkbEditarPagador.Enabled = !camposDistintos;

            TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado.CssClass = camposDistintos ? "ctxt" : "ctxtObr";
            TextBoxWatermarkExtender15.WatermarkCssClass = camposDistintos ? "ctxtMD" : "ctxtMDObr";

            TxtNome_Pagador_ConhecimentoOtimizado.CssClass = camposDistintos ? "ctxt" : "ctxtObr";
            TextBoxWatermarkExtender12.WatermarkCssClass = camposDistintos ? "ctxtMD" : "ctxtMDObr";
        }

        #region [Cubagem]


        private void CalcularPesoCubagem()
        {
            int contador = this.GvwConhecimentoComponentesCubagem.Rows.Count;
            if (contador > 0)
            {
                TextBox txtQuantidade;
                TextBox txtAltura;
                TextBox txtComprimento;
                TextBox txtFatorCubagem;
                TextBox txtLargura;

                double total = 0;
                double altura = 0;
                double largura = 0;
                double comprimento = 0;
                double fatorCubagem = 0;
                int quantidade = 0;

                for (int i = 0; contador > i; i++)
                {
                    GridViewRow gvr = this.GvwConhecimentoComponentesCubagem.Rows[i];
                    txtQuantidade = (TextBox)gvr.FindControl("txtQuantidade_ConhecimentoComponentesCubagem");
                    txtAltura = (TextBox)gvr.FindControl("txtAltura_ConhecimentoComponentesCubagem");
                    txtComprimento = (TextBox)gvr.FindControl("txtComprimento_ConhecimentoComponentesCubagem");
                    txtFatorCubagem = (TextBox)gvr.FindControl("txtFatorCubagem_ConhecimentoComponentesCubagem");
                    txtLargura = (TextBox)gvr.FindControl("txtLargura_ConhecimentoComponentesCubagem");


                    altura = String.IsNullOrEmpty(txtAltura.Text) ? 0 : _objTools.FieldDouble(txtAltura.Text);
                    largura = String.IsNullOrEmpty(txtLargura.Text) ? 0 : _objTools.FieldDouble(txtLargura.Text);
                    comprimento = String.IsNullOrEmpty(txtComprimento.Text) ? 0 : _objTools.FieldDouble(txtComprimento.Text);
                    fatorCubagem = String.IsNullOrEmpty(txtFatorCubagem.Text) ? 0 : _objTools.FieldDouble(txtFatorCubagem.Text);
                    quantidade = String.IsNullOrEmpty(txtQuantidade.Text) ? 0 : _objTools.FieldInt(txtQuantidade.Text);

                    total += (altura * largura * comprimento * fatorCubagem * quantidade);


                }
                this.TxPTotalPesoCubadoNF_ConhecimentoOtimizado.Text = String.Format("{0:N4}", total);

                if (this.GridNotasFiscaisSelecionadas.Rows.Count > 0)
                {
                    Label lblIdFilial = (Label)this.GridNotasFiscaisSelecionadas.Rows[0].FindControl("IdFilial");
                    if (lblIdFilial != null)
                    {
                        int idFilial = this._objTools.FieldInt(lblIdFilial.Text);

                        if (idFilial > 0)
                            ControlesWeb.FormatarPesoVolume(Page, idFilial);
                    }
                }
            }
        }
        protected void BtnSalvarCubagem_Click(object sender, EventArgs e)
        {
            CalcularPesoCubagem();
            this.SalvarCTRCxComponenteCubagemMemoria(true);
            MpeComposicaoCubagem.Hide();
        }
        protected void BtnFecharModalCubagem_Click(object sender, EventArgs e)
        {
            MpeComposicaoCubagem.Hide();
        }
        protected void BtnCalcularComponentePesoCubado_Click(object sender, EventArgs e)
        {
            this.SalvarCTRCxComponenteCubagemMemoria(true);
            MpeComposicaoCubagem.Show();
        }

        protected void BtnAdicionarComponente_Click(object sender, EventArgs e)
        {
            try
            {
                ConhecimentoComponentesCubagem objConhecimentoComponentesCubagem = new ConhecimentoComponentesCubagem();
                ControlesWeb.EnviarCamposParaInstancia(Page, objConhecimentoComponentesCubagem);
                PaginaWeb.preencherCamposAuditoria(objConhecimentoComponentesCubagem, Page);

                Cliente objCliente = new Cliente();
                objCliente.GetByCodigo(this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text);

                SalvarCTRCxComponenteCubagemMemoria(true);

                DataTable dt = objDtComponentesCubagem;

                DataRow dr = dt.NewRow();
                dr["IdConhecimentoComponentesCubagem"] = 0;
                dr["IdCTRC"] = 0;
                dr["Quantidade"] = 0;
                dr["Altura"] = 0;
                dr["Largura"] = 0;
                dr["Comprimento"] = 0;
                dr["FatorCubagem"] = objCliente.FatorCubagem;
                dr["TotalCubagem"] = 0;
                dr["IdUltimoUsuario"] = PaginaWeb.GetIdUsuarioCorrente(Page);
                dr["UltimaAtualizacao"] = DateTime.Now.ToString();

                dt.Rows.InsertAt(dr, 0);

                DataView dv = dt.DefaultView;
                dv.Sort = "UltimaAtualizacao Desc";
                dt = dv.ToTable();

                objDtComponentesCubagem = dt;
                this.GvwConhecimentoComponentesCubagem.DataSource = objDtComponentesCubagem;
                this.GvwConhecimentoComponentesCubagem.DataBind();
                this.MpeComposicaoCubagem.Show();
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void BtnRemoverComponente_Click(object sender, EventArgs e)
        {
            try
            {
                int contador = this.GvwConhecimentoComponentesCubagem.Rows.Count;
                if (contador > 0)
                {
                    CheckBox chkSelecionado = null;
                    Label lblId = null;
                    Label lblDescAltura = null;

                    SalvarCTRCxComponenteCubagemMemoria(false);

                    for (int i = (contador - 1); i >= 0; i--)
                    {
                        GridViewRow gvr = this.GvwConhecimentoComponentesCubagem.Rows[i];
                        chkSelecionado = (CheckBox)gvr.FindControl("chkselect");
                        lblId = (Label)gvr.FindControl("IdConhecimentoComponentesCubagem");
                        lblDescAltura = (Label)gvr.FindControl("Altura");

                        if (lblId != null)
                        {
                            if (chkSelecionado.Checked)
                            {
                                DataTable dt = objDtComponentesCubagem;
                                DataRow[] drResult = dt.Select("IdConhecimentoComponentesCubagem = " + lblId.Text);
                                if (drResult.Length > 0)
                                {
                                    if (Convert.ToInt32(lblId.Text) > 0)
                                    {
                                        ConhecimentoComponentesCubagem objConhecimentoComponentesCubagem = new ConhecimentoComponentesCubagem();
                                        objConhecimentoComponentesCubagem.GetById(Convert.ToInt32(lblId.Text));
                                        PaginaWeb.preencherCamposAuditoria(objConhecimentoComponentesCubagem, Page);
                                        objConhecimentoComponentesCubagem.Excluir();
                                        drResult[0].Delete();
                                    }
                                    if (Convert.ToInt32(lblId.Text) == 0)
                                        drResult[i].Delete();

                                }
                                objDtComponentesCubagem = dt;
                            }
                        }
                    }
                    GvwConhecimentoComponentesCubagem.DataSource = objDtComponentesCubagem;
                    GvwConhecimentoComponentesCubagem.DataBind();
                    CalcularPesoCubagem();
                    this.MpeComposicaoCubagem.Show();
                }
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
                return;
            }
        }

        private void SalvarCTRCxComponenteCubagemMemoria(bool atualizaGridView)
        {
            DataTable dt = objDtComponentesCubagem;

            int contador = this.objDtComponentesCubagem.Rows.Count;

            dt.Rows.Clear();

            if (contador > 0)
            {
                Label lblId = null;
                TextBox txtQuantidade = null;
                TextBox txtAltura = null;
                TextBox txtLargura = null;
                TextBox txtFatorCubagem = null;
                TextBox txtComprimento = null;

                for (int i = 0; contador > i; i++)
                {
                    GridViewRow gvr = this.GvwConhecimentoComponentesCubagem.Rows[i];
                    txtQuantidade = (TextBox)gvr.FindControl("txtQuantidade_ConhecimentoComponentesCubagem");
                    txtAltura = (TextBox)gvr.FindControl("txtAltura_ConhecimentoComponentesCubagem");
                    txtComprimento = (TextBox)gvr.FindControl("txtComprimento_ConhecimentoComponentesCubagem");
                    txtFatorCubagem = (TextBox)gvr.FindControl("txtFatorCubagem_ConhecimentoComponentesCubagem");
                    txtLargura = (TextBox)gvr.FindControl("txtLargura_ConhecimentoComponentesCubagem");
                    lblId = (Label)gvr.FindControl("IdConhecimentoComponentesCubagem");


                    DataRow dr = dt.NewRow();
                    dr["IdConhecimentoComponentesCubagem"] = lblId.Text;
                    dr["IdCTRC"] = 0;
                    dr["Quantidade"] = String.IsNullOrEmpty(txtQuantidade.Text) ? 0 : _objTools.FieldInt(txtQuantidade.Text);
                    dr["Altura"] = String.IsNullOrEmpty(txtAltura.Text) ? 0 : _objTools.FieldDouble(txtAltura.Text);
                    dr["Largura"] = String.IsNullOrEmpty(txtLargura.Text) ? 0 : _objTools.FieldDouble(txtLargura.Text);
                    dr["Comprimento"] = String.IsNullOrEmpty(txtComprimento.Text) ? 0 : _objTools.FieldDouble(txtComprimento.Text);
                    dr["FatorCubagem"] = String.IsNullOrEmpty(txtFatorCubagem.Text) ? 0 : _objTools.FieldDouble(txtFatorCubagem.Text);
                    dr["TotalCubagem"] = _objTools.FieldDouble(dr["Altura"]) * _objTools.FieldDouble(dr["Largura"]) * _objTools.FieldDouble(dr["Comprimento"]) * _objTools.FieldDouble(dr["FatorCubagem"]) * _objTools.FieldInt(dr["Quantidade"]);
                    dr["IdUltimoUsuario"] = PaginaWeb.GetIdUsuarioCorrente(Page);
                    dr["UltimaAtualizacao"] = DateTime.Now.ToString();
                    dt.Rows.Add(dr);
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "UltimaAtualizacao Desc";
                dt = dv.ToTable();
            }

            objDtComponentesCubagem = dt;

            if (atualizaGridView)
            {
                this.GvwConhecimentoComponentesCubagem.DataSource = objDtComponentesCubagem;
                this.GvwConhecimentoComponentesCubagem.DataBind();
            }
        }
        private void salvarCTRCxComponente()
        {
            if (objConhecimentoOtimizado.IdConhecimentoOtimizado == 0)
                return;

            Conhecimento _objConhecimento = new Conhecimento();
            _objConhecimento.GetById(objConhecimentoOtimizado.IdConhecimentoOtimizado);

            int contador = this.GvwConhecimentoComponentesCubagem.Rows.Count;
            if (contador > 0)
            {
                Label lblId = null;
                TextBox txtQuantidade = null;
                TextBox txtAltura = null;
                TextBox txtLargura = null;
                TextBox txtFatorCubagem = null;
                TextBox txtComprimento = null;
                bool atualizouComponentes = false;

                for (int i = 0; contador > i; i++)
                {
                    GridViewRow gvr = this.GvwConhecimentoComponentesCubagem.Rows[i];
                    txtQuantidade = (TextBox)gvr.FindControl("txtQuantidade_ConhecimentoComponentesCubagem");
                    txtAltura = (TextBox)gvr.FindControl("txtAltura_ConhecimentoComponentesCubagem");
                    txtComprimento = (TextBox)gvr.FindControl("txtComprimento_ConhecimentoComponentesCubagem");
                    txtFatorCubagem = (TextBox)gvr.FindControl("txtFatorCubagem_ConhecimentoComponentesCubagem");
                    txtLargura = (TextBox)gvr.FindControl("txtLargura_ConhecimentoComponentesCubagem");
                    lblId = (Label)gvr.FindControl("IdConhecimentoComponentesCubagem");

                    if ((lblId != null))
                    {
                        ConhecimentoComponentesCubagem objConhecimentoComponentesCubagem = new ConhecimentoComponentesCubagem();
                        objConhecimentoComponentesCubagem.GetById(objClassSQLBasico.Tools.FieldInt(lblId.Text));
                        objConhecimentoComponentesCubagem.IdCTRC = _objConhecimento.IdConhecimento;
                        objConhecimentoComponentesCubagem.Quantidade = String.IsNullOrEmpty(txtQuantidade.Text) ? 0 : objClassSQLBasico.Tools.FieldInt(txtQuantidade.Text);
                        objConhecimentoComponentesCubagem.Largura = String.IsNullOrEmpty(txtLargura.Text) ? 0 : objClassSQLBasico.Tools.FieldDouble(txtLargura.Text);
                        objConhecimentoComponentesCubagem.Altura = String.IsNullOrEmpty(txtAltura.Text) ? 0 : objClassSQLBasico.Tools.FieldDouble(txtAltura.Text);
                        objConhecimentoComponentesCubagem.Comprimento = String.IsNullOrEmpty(txtComprimento.Text) ? 0 : objClassSQLBasico.Tools.FieldDouble(txtComprimento.Text);
                        objConhecimentoComponentesCubagem.FatorCubagem = String.IsNullOrEmpty(txtFatorCubagem.Text) ? 0 : objClassSQLBasico.Tools.FieldDouble(txtFatorCubagem.Text);
                        objConhecimentoComponentesCubagem.TotalCubagem = (objConhecimentoComponentesCubagem.FatorCubagem *
                            objConhecimentoComponentesCubagem.Comprimento * objConhecimentoComponentesCubagem.Altura *
                            objConhecimentoComponentesCubagem.Largura * objConhecimentoComponentesCubagem.Quantidade);

                        objConhecimentoComponentesCubagem.IdUltimoUsuario = PaginaWeb.GetIdUsuarioCorrente(Page);
                        objConhecimentoComponentesCubagem.UltimaAtualizacao = DateTime.Now.ToString();
                        PaginaWeb.preencherCamposAuditoria(objConhecimentoComponentesCubagem, Page);
                        objConhecimentoComponentesCubagem.Salvar();
                        atualizouComponentes = true;
                    }
                }

                if (atualizouComponentes)
                    this.atualizarGridComponentesCubagem();
            }
        }
        private void configuracaoComponentes()
        {
            objDtComponentesCubagem = new DataTable();
            objDtComponentesCubagem.Columns.Clear();
            objDtComponentesCubagem.Columns.Add("IdConhecimentoComponentesCubagem", typeof(int));
            objDtComponentesCubagem.Columns.Add("IdCTRC", typeof(int));
            objDtComponentesCubagem.Columns.Add("Quantidade", typeof(int));
            objDtComponentesCubagem.Columns.Add("Altura", typeof(double));
            objDtComponentesCubagem.Columns.Add("Largura", typeof(double));
            objDtComponentesCubagem.Columns.Add("Comprimento", typeof(double));
            objDtComponentesCubagem.Columns.Add("FatorCubagem", typeof(double));
            objDtComponentesCubagem.Columns.Add("TotalCubagem", typeof(double));
            objDtComponentesCubagem.Columns.Add("IdUltimoUsuario", typeof(int));
            objDtComponentesCubagem.Columns.Add("UltimaAtualizacao", typeof(string));

        }
        private DataTable objDtComponentesCubagem
        {
            get
            {
                return (DataTable)ViewState["objDtComponentesCubagem"];
            }
            set
            {
                ViewState["objDtComponentesCubagem"] = value;
            }
        }

        private void atualizarGridComponentesCubagem()
        {
            try
            {
                if (objConhecimentoOtimizado.IdConhecimentoOtimizado > 0)
                    objDtComponentesCubagem = new ClassSQLCarga().ListaComponenteCTRB(objConhecimentoOtimizado.IdConhecimentoOtimizado).Tables[0];

                this.GvwConhecimentoComponentesCubagem.DataSource = objDtComponentesCubagem;
                this.GvwConhecimentoComponentesCubagem.DataBind();
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        private bool verifcarControlePallet()
        {
            try
            {
                NotaFiscalList listaNotaFiscal = ControleGridView.getNotaFiscalList(this.GridNotasFiscaisSelecionadas);
                Cliente objRemetente = new Cliente();
                objRemetente.GetById(listaNotaFiscal.NotaFiscal.IdRemetente);
                bool NaoPossuiControlePallet = false;
                if (objRemetente.IncluidoNoControleDeDevolucaoDePallet == (byte)eSimNao.Sim && objRemetente.ConsiderarNotaFiscalDePalletNoCalculoDoFrete == (byte)eSimNao.Sim)
                {
                    NaoPossuiControlePallet = true;
                    while (listaNotaFiscal.Read())
                    {
                        NotaFiscal objNotaFiscal = new NotaFiscal();
                        objNotaFiscal = listaNotaFiscal.NotaFiscal;
                        int idNatureza = objNotaFiscal.IdNatureza;
                        Natureza objNatureza = new Natureza();
                        objNatureza.GetById(idNatureza);
                        if (objNatureza.ControlaPallet == (byte)eSimNao.Sim)
                        {
                            NaoPossuiControlePallet = false;
                            return NaoPossuiControlePallet;
                        }

                    }
                }

                return NaoPossuiControlePallet;
            }
            catch (Exception erro)
            {
                return true;
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);

            }
        }

        protected void GvwListaConhecimentoComponentesCubagem_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", "ChangeColor('" + "GvwConhecimentoComponentesCubagem','" + (e.Row.RowIndex + 1).ToString() + "')");

                    Label lblIdConhecimentoComponentesCubagem = (Label)e.Row.FindControl("IdConhecimentoComponentesCubagem");
                }
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void ChkConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05.Checked)
            {
                this.LblPesoCubado_NotaFiscal.Visible = false;
                this.TxPPesoCubado_NotaFiscal.Visible = false;
                this.LblPesoCubadoTotal.Visible = false;
                this.TxPTotalPesoCubadoNF_ConhecimentoOtimizado.Visible = false;
                ControlesWeb.TornarCampoGridVisivel(this.GvwListaNotasDisponiveis, "Peso Cubado", false);
                ControlesWeb.TornarCampoGridVisivel(this.GridNotasFiscaisSelecionadas, "Peso Cubado", false);

                this.BtnComporPesoCubado.Visible = false;
                this.BtnComporPesoCubadoTotal.Visible = false;
            }
            else
            {
                this.LblPesoCubado_NotaFiscal.Visible = true;
                this.TxPPesoCubado_NotaFiscal.Visible = true;
                this.LblPesoCubadoTotal.Visible = true;
                this.TxPTotalPesoCubadoNF_ConhecimentoOtimizado.Visible = true;
                ControlesWeb.TornarCampoGridVisivel(this.GvwListaNotasDisponiveis, "Peso Cubado", true);
                ControlesWeb.TornarCampoGridVisivel(this.GridNotasFiscaisSelecionadas, "Peso Cubado", true);

                this.BtnComporPesoCubado.Visible = true;
                this.BtnComporPesoCubadoTotal.Visible = true;
            }



        }

        protected void GvwConhecimentoComponentesCubagem_PreRender(object sender, EventArgs e)
        {

        }

        #endregion

        protected void BtnTratarTipoOperacao_Click(object sender, EventArgs e)
        {
            try
            {
                DdlTipoOperacaoTransporte_ConhecimentoOtimizado.Enabled = pnlTipoOperacaoTransporte.Enabled;
                tratarTipoOperacao();

                //Ao alterar o tipoOperacao, fazer tratativas do Pallet
                this.BtnCalcularTotalPallets_Click(sender, e);
            }
            catch (Exception erro)
            {
                LblMensagem.Text = erro.Message;
                LblMensagem.ForeColor = Color.Red;
            }
        }

        protected void BtnCalcularTotalPallets_Click(object sender, EventArgs e)
        {
            try
            {
                tratarCamposPallet();
                computarTotalPalletsNotaNaturezaPaletizada();
            }
            catch (Exception erro)
            {
                LblMensagem.Text = erro.Message;
                LblMensagem.ForeColor = Color.Red;
            }
        }

        protected void RbtGeracaoCtePorNota_Click(object sender, EventArgs e)
        {
            this.BtnLimparConsultaCteTerceiro_Click(sender, e);
            this.TrataVisualGridNotasCTe();
            this.FiltraPesquisa();

        }

        private void TrataGeracaoCTePorNota(object sender, EventArgs e)
        {
            try
            {
                bool primeiraNota = this.GridNotasFiscaisSelecionadas.Rows.Count == 0;
                verificarNotaJaVinculadaConhecimentoMesmoTipo();
                ControleGridView.MigrarDisponiveisChecadasParaSelecionadas(this.GvwListaNotasDisponiveis, this.GridNotasFiscaisSelecionadas, true);
                AtualizarInformacoesControlesWeb();
                if (this.GridNotasFiscaisSelecionadas.Rows.Count > 0)
                {
                    ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
                    objConfiguraModulo.Get();

                    if (objConfiguraModulo.ConsiderarCifFobOriundoNotaFiscalCadastroOtimizadoCTE != (byte)eSimNao.Nao)
                        CifFob_NotaFiscal_Click(sender, e);
                    else if (objConfiguraModulo.SugerirTomadorFreteCadastroOtimizado != 0)
                        CifFob_ConfiguraModulo_Click(sender, e);
                    else
                        CifFob_Click(sender, e);

                    if (objConfiguraModulo.UtilizarControleRotaCadastroOtimizado == (byte)eSimNao.Sim)
                        this.sugerirRotaCTeCadastroOtimizado();

                    this.AtualizarControlesNFsRelacionadas();
                    this.AtualizarVolumeTotal();

                    Label _IdNotaFiscal = (Label)GridNotasFiscaisSelecionadas.Rows[0].FindControl("IdNotaFiscal");
                    this.IdNotaFiscalRemetente = _objTools.ConvertToInt32(_IdNotaFiscal.Text);


                }

                if (primeiraNota && this.GridNotasFiscaisSelecionadas.Rows.Count != 0)
                {
                    aplicaMascaraPeso();
                }

                computarTotalPalletsNotaNaturezaPaletizada();
                tratarTipoOperacao();
                tratarCamposPallet();
            }
            catch (Exception erro)
            {

                throw new Exception(erro.Message);
            }
        }

        private void AtualizarInformacoesControlesWebCTePorNota()
        {
            try
            {
                LblNotasFiscaisDisponiveis.Text = "Notas Fiscais Disponíveis (" + this.GvwListaNotasDisponiveis.Rows.Count + ")";
                LblNotasFiscaisSelecionadas.Text = "Notas Fiscais Selecionadas (" + this.GridNotasFiscaisSelecionadas.Rows.Count + ")";
                PnlGerarConhecimento.Enabled = (this.GridNotasFiscaisSelecionadas.Rows.Count > 0);

                if (this.DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado.SelectedIndex == 0)
                {
                    NotaFiscalList objNotaFiscalList = ControleGridView.getNotaFiscalList(this.GridNotasFiscaisSelecionadas);
                    if ((objNotaFiscalList.Count() > 0) && (this.GridNotasFiscaisSelecionadas.Rows.Count > 0))
                    {
                        ConhecimentoOtimizado objConhecimentoOtimizado = new ConhecimentoOtimizado();
                        objConhecimentoOtimizado.SugerirCadastro(objNotaFiscalList.NotaFiscal);
                        ControlesWeb.RelacionaObjetoClasseControlesPagina(objConhecimentoOtimizado, Page.Controls);
                        AtualizaTipoTransporte(objNotaFiscalList.NotaFiscal);
                    }
                }
                else
                {
                    if (this.GridNotasFiscaisSelecionadas.Rows.Count == 0)
                    {
                        ConhecimentoOtimizado objConhecimentoOtimizado = new ConhecimentoOtimizado();
                        ControlesWeb.RelacionaObjetoClasseControlesPagina(objConhecimentoOtimizado, Page.Controls);
                    }
                }
            }
            catch (Exception erro)
            {

                throw new Exception(erro.Message);
            }

        }

        private void TrataVisualGridNotasCTe()
        {
            if (!this.RbtGeracaoCtePorNota.Checked)
            {
                this.GvwListaNotasDisponiveis.DataSource = null;
                this.GvwListaNotasDisponiveis.DataBind();
                this.GvwListaNotasDisponiveis.Visible = false;
                this.GridNotasFiscaisSelecionadas.DataSource = null;
                this.GridNotasFiscaisSelecionadas.DataBind();
                this.GridNotasFiscaisSelecionadas.Visible = false;
                this.Notas.Visible = false;
                this.DivGridNotasFiscaisSelecionadas.Visible = false;


                this.DivCTeTerceiroDisponiveis.Visible = true;
                this.DivCTeTerceiroSelecionadas.Visible = true;

                this.GvwListaCTeTerceiroDisponiveis.Visible = true;
                this.GvwListaCTeTerceiroSelecionadas.Visible = true;
                this.DdlTipoServicoCte_ConhecimentoOtimizado.SelectedIndex = 1;
                this.DdlTipoServicoCte_ConhecimentoOtimizado.Enabled = false;
                this.BtnOpcoesFiltro.Enabled = true;
                this.BtnRetirarInserirNF.Enabled = false;
                this.BtnCadastrarNovaNota.Enabled = false;
                this.BtnImportarXMLChave.Enabled = false;

                this.LblNotasFiscaisDisponiveis.Text = "CT-e de Terceiros Disponíveis (" + this.GvwListaCTeTerceiroDisponiveis.Rows.Count + ")";
                this.LblNotasFiscaisSelecionadas.Text = "CT-e de Terceiros Selecionados (" + this.GvwListaCTeTerceiroSelecionadas.Rows.Count + ")";

            }
            else
            {
                this.GvwListaNotasDisponiveis.Visible = true;
                this.GridNotasFiscaisSelecionadas.Visible = true;
                this.Notas.Visible = true;
                this.DivGridNotasFiscaisSelecionadas.Visible = true;

                this.DivCTeTerceiroDisponiveis.Visible = false;
                this.DivCTeTerceiroSelecionadas.Visible = false;

                this.GvwListaCTeTerceiroDisponiveis.DataSource = null;
                this.GvwListaCTeTerceiroDisponiveis.DataBind();
                this.GvwListaCTeTerceiroDisponiveis.Visible = false;

                this.GvwListaCTeTerceiroSelecionadas.DataSource = null;
                this.GvwListaCTeTerceiroSelecionadas.DataBind();
                this.GvwListaCTeTerceiroSelecionadas.Visible = false;

                this.DdlTipoServicoCte_ConhecimentoOtimizado.SelectedIndex = 0;
                this.DdlTipoServicoCte_ConhecimentoOtimizado.Enabled = true;
                this.BtnOpcoesFiltro.Enabled = true;
                this.BtnRetirarInserirNF.Enabled = true;
                this.BtnCadastrarNovaNota.Enabled = true;
                this.BtnImportarXMLChave.Enabled = true;

                this.LblNotasFiscaisDisponiveis.Text = "Notas Fiscais Disponíveis (" + this.GvwListaNotasDisponiveis.Rows.Count + ")";
                this.LblNotasFiscaisSelecionadas.Text = "Notas Fiscais Selecionadas (" + this.GridNotasFiscaisSelecionadas.Rows.Count + ")";
            }

            this.ExibirTipoConsulta();

        }

        private void ExibirTipoConsulta()
        {
            if (this.RbtGeracaoCtePorNota.Checked)
            {
                //IdConhecimentoOtimizado = 0;
                #region[ DdlConsultar ]
                this.DdlTipoConsulta.Items.Clear();
                this.DdlTipoConsulta.Items.Add(new ListItem("Apenas Notas não associadas a CTRC e NFS", "0"));
                this.DdlTipoConsulta.Items.Add(new ListItem("Apenas Notas já associadas a um CTRC ou NFS", "1"));
                this.DdlTipoConsulta.Items.Add(new ListItem("Todas as Notas", "2"));
                this.DdlTipoConsulta.DataBind();
                #endregion
                #region[ DdlTipoNotaXML ]
                this.DdlTipoNotaXML.Items.Clear();
                this.DdlTipoNotaXML.AppendDataBoundItems = true;
                this.DdlTipoNotaXML.Items.Insert(0, "");
                this.DdlTipoNotaXML.DataSource = objTiposConstantes.TipoNotasTerceiros();
                this.DdlTipoNotaXML.DataValueField = objTiposConstantes.DataValueField;
                this.DdlTipoNotaXML.DataTextField = objTiposConstantes.DataTextField;
                this.DdlTipoNotaXML.DataBind();
                #endregion
                #region[ DdlNatureza ]
                atualizarNatureza();
                #endregion
                #region[ DdlEspecie ]
                atualizarEspecie();
                #endregion
            }

            else
            {
                #region[ DdlTipoConsultaCteTerceiro ]
                this.DdlTipoConsultaCteTerceiro.Items.Clear();
                this.DdlTipoConsultaCteTerceiro.Items.Add(new ListItem("Apenas por CT-e Terc. não associados a CT-e", "0"));
                this.DdlTipoConsultaCteTerceiro.Items.Add(new ListItem("Apenas por CT-e Terc. já associados a CT-e", "1"));
                this.DdlTipoConsultaCteTerceiro.Items.Add(new ListItem("Todos CT-e de Terceiros", "2"));
                this.DdlTipoConsultaCteTerceiro.DataBind();
                #endregion
            }
        }

        #region[Metodos CT-e terceiro]
        public ClassSQLCarga.stParametrosCTeTerceiro PopularObjParametrosCTeTerceiro()
        {
            ClassSQLCarga.stParametrosCTeTerceiro objstParametros = new ClassSQLCarga.stParametrosCTeTerceiro();
            try
            {


                objstParametros.Filial = _objTools.FieldInt(this.DdlIdFilial_Filial_CteTerceiroConsulta.SelectedValue);
                objstParametros.NumeroCTeTerceiro = this.TxtNumeroCTeTerceiro_CteTerceiroConsulta.Text.Trim();
                objstParametros.DataEmissao = this.TxtDataEmissaoConsultaInicialCteTerceiro.Text;
                objstParametros.CodEmitente = _objTools.FieldInt(this.TxtCodCliente_Emitente_CteTerceiroConsulta.Text.Trim());
                objstParametros.Emitente = this.TxtNome_Emitente_CteTerceiroConsulta.Text.Trim();
                objstParametros.CodRemetente = _objTools.FieldInt(this.TxtCodCliente_Remetente_CteTerceiroConsulta.Text.Trim());
                objstParametros.Remetente = this.TxtNome_Remetente_CteTerceiroConsulta.Text.Trim();
                objstParametros.CodDestinatario = _objTools.FieldInt(this.TxtCodCliente_Destinatario_CteTerceiroConsulta.Text.Trim());
                objstParametros.Destinatario = this.TxtNome_Destinatario_CteTerceiroConsulta.Text.Trim();
                objstParametros.LocalColeta = string.IsNullOrEmpty(this.TxtUF_MunicipioColeta_CteTerceiroConsulta.Text) ? "" : (this.TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta.Text.Trim() + "\\" + TxtUF_MunicipioColeta_CteTerceiroConsulta.Text.Trim());
                objstParametros.LocalEntrega = string.IsNullOrEmpty(this.TxtUF_MunicipioEntrega_CteTerceiroConsulta.Text) ? "" : (this.TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta.Text.Trim() + "\\" + TxtUF_MunicipioEntrega_CteTerceiroConsulta.Text.Trim());
                objstParametros.ChaveEletronicaCTeTerceiro = this.TxtChaveEletronicaCTeTerceiro_CTeTerceiroConsulta.Text.Trim();
                objstParametros.NumeroCarga = TxtNumeroCarga_CteTerceiroConsulta.Text.Trim();
                if (Session["idUsuarioLog"] != null)
                {
                    objstParametros.UsuarioLog = _objTools.FieldInt(Session["idUsuarioLog"]);
                }




            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
            return objstParametros;
        }

        protected void RbtGeracaoCtePorCte_Click(object sender, EventArgs e)
        {
            this.BtnLimparConsultaNotaFiscal_Click(sender, e);
            ClassSQLCarga.stParametrosCTeTerceiro objstParametros = new ClassSQLCarga.stParametrosCTeTerceiro();
            if (Session["idUsuarioLog"] != null)
            {
                objstParametros.UsuarioLog = _objTools.FieldInt(Session["idUsuarioLog"]);
            }
            this.TrataVisualGridNotasCTe();
            this.FiltraPesquisa();
            this.DdlTipoServicoCte_ConhecimentoOtimizado.SelectedIndex = 1;
            this.DdlTipoServicoCte_ConhecimentoOtimizado.Enabled = false;

        }

        protected void EditarConhecimentoTerceiro_Command(object sender, CommandEventArgs e)
        {
            try
            {

                int _idConhecimentoTerceiro = Convert.ToInt32(e.CommandArgument.ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "ConhecimentoTerceiro", "<script type='text/javascript' language='javascript'>Editar('" + _idConhecimentoTerceiro.ToString() + "','../ConhecimentoTerceiro/ConhecimentoTerceiro.aspx');</script>", false);
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        private void TrataGeracaoCTePorCTeTerceiro(object sender, EventArgs e)
        {
            try
            {
                this.DdlTipoServicoCte_ConhecimentoOtimizado.SelectedIndex = 1;
                this.DdlTipoServicoCte_ConhecimentoOtimizado.Enabled = false;
                ControleGridView.MigrarCteTerceiroDisponiveisChecadasParaSelecionadas(this.GvwListaCTeTerceiroDisponiveis, this.GvwListaCTeTerceiroSelecionadas, this.RbtGeracaoCtePorCte.Checked);
                AtualizarInformacoesControlesWeb();
                this.Rb3CifFob_ConhecimentoOtimizado.Checked = true;
                CifFob_Click(sender, e);


            }
            catch (Exception erro)
            {

                throw new Exception(erro.Message);
            }
        }

        private void AtualizarInformacoesControlesWebCTePorCTeTerceiro()
        {
            try
            {
                LblNotasFiscaisDisponiveis.Text = "CT-e de Terceiros Disponíveis (" + this.GvwListaCTeTerceiroDisponiveis.Rows.Count + ")";
                LblNotasFiscaisSelecionadas.Text = "CT-e de Terceiros Selecionados (" + this.GvwListaCTeTerceiroSelecionadas.Rows.Count + ")";
                PnlGerarConhecimento.Enabled = (this.GvwListaCTeTerceiroSelecionadas.Rows.Count > 0);



                ConhecimentoTerceiroList objConhecimentoTerceiroList = ControleGridView.GetConhecimentoTerceiroList(this.GvwListaCTeTerceiroSelecionadas);
                if (objConhecimentoTerceiroList.Count() > 0 && this.GvwListaCTeTerceiroSelecionadas.Rows.Count > 0)
                {
                    ConhecimentoOtimizado objConhecimentoOtimizado = new ConhecimentoOtimizado();
                    objConhecimentoOtimizado.SugerirCadastroCTeTerceiro(objConhecimentoTerceiroList.ConhecimentoTerceiro);
                    ControlesWeb.RelacionaObjetoClasseControlesPagina(objConhecimentoOtimizado, Page.Controls);

                }


                if (this.GvwListaCTeTerceiroSelecionadas.Rows.Count == 0)
                {
                    ConhecimentoOtimizado objConhecimentoOtimizado = new ConhecimentoOtimizado();
                    ControlesWeb.RelacionaObjetoClasseControlesPagina(objConhecimentoOtimizado, Page.Controls);
                }



            }
            catch (Exception erro)
            {

                throw new Exception(erro.Message);
            }

        }

        private void TrataCifFobGeracaoCTePorNota(object sender, EventArgs e)
        {
            try
            {
                int idNotaFiscal = 0;
                DataTable dtTodasSelecionadas = ControleGridView.TransformarGridViewInDataTable(this.GridNotasFiscaisSelecionadas, false);
                if (dtTodasSelecionadas != null)
                {
                    if (dtTodasSelecionadas.Rows.Count > 0)
                        idNotaFiscal = Convert.ToInt32(dtTodasSelecionadas.Rows[0]["IdNotaFiscal"]);

                    dtTodasSelecionadas.Dispose();
                }

                NotaFiscal objNotaFiscal = new NotaFiscal();
                objNotaFiscal.GetById(idNotaFiscal);

                if (this.Rb1CifFob_ConhecimentoOtimizado.Checked)
                {
                    this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Remetente.CodCliente.ToString().Trim();
                    this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Remetente.CnpjCpf.ToString().Trim();
                    LkbAtualizarPagador_Click(sender, e);
                }
                else
                {
                    if (this.Rb2CifFob_ConhecimentoOtimizado.Checked)
                    {
                        this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Destinatario.CodCliente.ToString().Trim();
                        this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objNotaFiscal.Destinatario.CnpjCpf.ToString().Trim();
                        LkbAtualizarPagador_Click(sender, e);
                    }
                    else
                    {
                        if (this.Rb3CifFob_ConhecimentoOtimizado.Checked)
                        {
                            this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = "";
                            this.TxtNome_Pagador_ConhecimentoOtimizado.Text = "";
                            this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = "";
                        }
                    }
                }
            }
            catch (Exception erro)
            {

                throw new Exception(erro.Message);
            }
        }

        private void TrataCifFobGeracaoCTePorCTeTerceiro(object sender, EventArgs e)
        {
            try
            {
                int idConhecimentoTerceiro = 0;
                DataTable dtTodasSelecionadas = ControleGridView.TransformarCTeTerceiroGridViewInDataTable(this.GvwListaCTeTerceiroSelecionadas, false, this.RbtGeracaoCtePorCte.Checked);
                if (dtTodasSelecionadas != null)
                {
                    if (dtTodasSelecionadas.Rows.Count > 0)
                        idConhecimentoTerceiro = Convert.ToInt32(dtTodasSelecionadas.Rows[0]["idConhecimentoTerceiro"]);

                    dtTodasSelecionadas.Dispose();


                }

                ConhecimentoTerceiro objConhecimentoTerceiro = new ConhecimentoTerceiro();
                objConhecimentoTerceiro.GetById(idConhecimentoTerceiro);

                if (this.Rb1CifFob_ConhecimentoOtimizado.Checked)
                {
                    this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objConhecimentoTerceiro.Remetente.CodCliente.ToString().Trim();
                    this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objConhecimentoTerceiro.Remetente.CnpjCpf.ToString().Trim();
                    LkbAtualizarPagador_Click(sender, e);
                }
                else
                {
                    if (this.Rb2CifFob_ConhecimentoOtimizado.Checked)
                    {
                        this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objConhecimentoTerceiro.Destinatario.CodCliente.ToString().Trim();
                        this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objConhecimentoTerceiro.Destinatario.CnpjCpf.ToString().Trim();
                        LkbAtualizarPagador_Click(sender, e);
                    }
                    else
                    {
                        if (this.Rb3CifFob_ConhecimentoOtimizado.Checked)
                        {
                            this.TxtCodCliente_Pagador_ConhecimentoOtimizado.Text = objConhecimentoTerceiro.Emitente.CodCliente.ToString().Trim();
                            this.TxtCnpjCpf_Pagador_ConhecimentoOtimizado.Text = objConhecimentoTerceiro.Emitente.CnpjCpf.ToString().Trim();
                            LkbAtualizarPagador_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception erro)
            {

                throw new Exception(erro.Message);
            }
        }

        private void ImportarArquivosCTeTerceiro()
        {
            this.LblMensagemImportacaoXMLCTeTerceiro.Text = "";
            int _idFilial = retornaItemSelecionado(this.DdlIdFilial_Filial_XMLCTeTerceiro);
            int _idUltimoUsuario = PaginaWeb.GetIdUsuarioCorrente(Page);
            string arquivoAnterior = "";

            /*string caminhoCTETerceiro = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"] + @"\AreaTransferencia\CTETerceiro" + "\\";*/

            #region [ Validações ]
            if (_idFilial == 0)
                throw new Exception("Informe a Filial");

            #endregion

            ImportaXMLNFeList.Clear();

            #region [ Busca no proprio componente ]
            try
            {
                HttpFileCollection hfc = Request.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFile hpf = hfc[i];

                    if (!string.IsNullOrEmpty(hpf.FileName))
                    {
                        if (hpf.FileName.ToUpper().EndsWith(".XML"))
                        {
                            if (!Directory.Exists(caminhoCTETerceiro))
                                Directory.CreateDirectory(caminhoCTETerceiro);

                            hpf.SaveAs(caminhoCTETerceiro + System.IO.Path.GetFileName(hpf.FileName));
                            ImportaXMLNFeList.Add(caminhoCTETerceiro + System.IO.Path.GetFileName(hpf.FileName));
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Importação não realizada! Nenhum arquivo XML de CT-e selecionado ou seleção ultrapassou 590 arquivos");
            }
            #endregion

            #region [ Se nao achou no componente pode ter sido listado antes ]
            if (ImportaXMLNFeList.Count == 0)
            {
                for (int i = 0; i < TextBoxList.Count; i++)
                {
                    string linha = (string)TextBoxList[i];
                    ImportaXMLNFeList.Add(caminhoCTETerceiro + System.IO.Path.GetFileName(linha));
                }
            }
            #endregion

            #region [ Processo de importacao ]
            ArrayList arrayList = ImportaXMLNFeList;

            if (arrayList == null)
                return;

            if (arrayList.Count == 0)
                throw new Exception("Importação não realizada! Nenhum arquivo XML de CT-e selecionado ou seleção ultrapassou 590 arquivos");

            bool todosArqImportados = true;

            #region [ Cria DataSet ]
            DataSet dsCTeTerceiro = (DataSet)Session["dsPopulado"];
            if (dsCTeTerceiro == null)
            {
                CriarDataSet();
                dsCTeTerceiro = (DataSet)Session["dsPopulado"];
            }
            #endregion

            for (int i = 0; i < arrayList.Count; i++)
            {
                string path = (string)arrayList[i];

                if (!string.IsNullOrEmpty(path))
                {
                    Servicelogic.TMS.Fiscal.ImportaCTeTerceiro objImportaCTeTerceiro = new Servicelogic.TMS.Fiscal.ImportaCTeTerceiro();
                    objImportaCTeTerceiro.ImportarArquivos(objImportaCTeTerceiro, path, _idUltimoUsuario.ToString().Trim(), _idFilial, "Cadastro Otimizado");

                    #region [ Preenche DataSet ]
                    DataRow dr = dsCTeTerceiro.Tables[0].NewRow();

                    dr["Filial"] = DdlIdFilial_Filial_XMLCTeTerceiro.SelectedItem.Text;
                    dr["Emitente"] = objImportaCTeTerceiro.Emitente;
                    dr["Arquivo"] = objImportaCTeTerceiro.Arquivo;
                    dr["ChaveCTe"] = objImportaCTeTerceiro.ChaveCTe;
                    dr["SerieCTRC"] = objImportaCTeTerceiro.SerieCTRC;
                    dr["NumCTRC"] = objImportaCTeTerceiro.NumCTRC;
                    dr["DataEmissao"] = objImportaCTeTerceiro.DataEmissao;
                    dr["Valor"] = objImportaCTeTerceiro.Valor;
                    dr["Mensagem"] = objImportaCTeTerceiro.Mensagem;
                    dsCTeTerceiro.Tables[0].Rows.Add(dr);
                    #endregion

                    arquivoAnterior = objImportaCTeTerceiro.Arquivo;
                    File.Delete(path);
                    arrayList[i] = "";
                    ImportaXMLNFeList = arrayList;
                    caminhoCTETerceiro = "";

                }
            }
            #endregion

            #region [ Mostra relatorio final ]
            TextBoxList.Clear();
            TxtListagemArquivoCTeTerceiro.Text = "";
            LblListarXMLCTeTerceiro.Text = "";
            #endregion


            if (todosArqImportados)
            {
                Session["dsPopulado"] = dsCTeTerceiro;
                ImportaXMLNFeList.Clear();
                dsCTeTerceiro.Dispose();

                #region [ Relatorio de Importacao ]
                //relatorio de resultado da importacao
                ScriptManager.RegisterStartupScript(this, typeof(Page), "mensagem", "<script type='text/javascript' language='javascript'>open('../../Pagina/ImportaCTeTerceiro/PrwImportaCTeTerceiro.aspx','PrwImportaCTeTerceiro')</script>", false);
                #endregion

            }
        }
        #endregion

        #region [ Metodos consulta\limpa e cancelar Ct-e Terceiro ]
        protected void BtnConsultarCteTerceirol_Click(object sender, EventArgs e)
        {
            try
            {

                FiltraPesquisa();
                LimparConsultaCteTerceiro();
                AtualizarInformacoesControlesWeb();
                if (GvwListaCTeTerceiroSelecionadas.Rows.Count > 0)
                {
                    this.Rb3CifFob_ConhecimentoOtimizado.Checked = true;
                }


                CifFob_Click(sender, e);
                this.TrataVisualGridNotasCTe();

                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void BtnLimparConsultaCteTerceiro_Click(object sender, EventArgs e)
        {
            try
            {
                LimparConsultaCteTerceiro();


            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
            }
        }

        protected void LimparConsultaCteTerceiro()
        {
            try
            {
                this.DdlTipoConsultaCteTerceiro.SelectedIndex = 0;
                this.DdlIdFilial_Filial_CteTerceiroConsulta.SelectedIndex = 0;
                this.TxtNumeroCTeTerceiro_CteTerceiroConsulta.Text = "";
                this.TxtDataEmissaoConsultaInicialCteTerceiro.Text = "";


                this.TxtCodCliente_Emitente_CteTerceiroConsulta.Text = "";
                this.TxtNome_Emitente_CteTerceiroConsulta.Text = "";
                this.TxtCnpjCpf_Emitente_CteTerceiroConsulta.Text = "";

                this.TxtCodCliente_Remetente_CteTerceiroConsulta.Text = "";
                this.TxtNome_Remetente_CteTerceiroConsulta.Text = "";
                this.TxtCnpjCpf_Remetente_CteTerceiroConsulta.Text = "";

                this.TxtCodCliente_Destinatario_CteTerceiroConsulta.Text = "";
                this.TxtNome_Destinatario_CteTerceiroConsulta.Text = "";
                this.TxtCnpjCpf_Destinatario_CteTerceiroConsulta.Text = "";

                this.TxtCodMunicipio_MunicipioColeta_CteTerceiroConsulta.Text = "";
                this.TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta.Text = "";
                this.TxtUF_MunicipioColeta_CteTerceiroConsulta.Text = "";

                this.TxtCodMunicipio_MunicipioEntrega_CteTerceiroConsulta.Text = "";
                this.TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta.Text = "";
                this.TxtUF_MunicipioEntrega_CteTerceiroConsulta.Text = "";

                this.TxtChaveEletronicaCTeTerceiro_CTeTerceiroConsulta.Text = "";
                this.TxtNumeroCarga_CteTerceiroConsulta.Text = "";
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }


        protected void BtnAbortarCteTerceiro_Click(object sender, EventArgs e)
        {
            try
            {
                LimparConsultaCteTerceiro();
                CancelarCteTerceiro();

                CifFob_Click(sender, e);


            }
            catch (Exception erro)
            {
                PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);
                return;
            }
        }

        protected void CancelarCteTerceiro()
        {
            try
            {
                AtualizarInformacoesControlesWeb();
                if (GvwListaCTeTerceiroSelecionadas.Rows.Count > 0)
                {
                    this.Rb3CifFob_ConhecimentoOtimizado.Checked = true;
                }
                                
                this.TrataVisualGridNotasCTe();
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;
            }
            catch (Exception erro)
            {
                this.mvwPrincipal.ActiveViewIndex = 0;
                this.mvwGeral.ActiveViewIndex = 0;

                throw new Exception(erro.Message);
                /*PaginaWeb.Mensagem(Page, erro.Message, TipoMensagem.Error);*/
            }
        }

        #endregion


    }
}

#region [ Classe de Controle dos GridViews ]

public static class ControleGridView
{

    #region [ Public ]


    public static DataTable ValidarDocumentosDisponiveisSelecionados(DataTable dtDisponiveis, GridView objGridViewSelecionadas, bool tipoGeracaoCTePorNota, bool tipoGeracaoCTePorCte)
    {

        if (objGridViewSelecionadas.Rows.Count == 0)
            return dtDisponiveis;

        if (dtDisponiveis.Rows.Count == 0)
            return dtDisponiveis;

        DataTable dtReturn = new DataTable();

        if (tipoGeracaoCTePorNota)
        {
            string ChaveDocumento = "ChaveEletronicaNFe";
            DataTable dtSelecionadas = TransformarGridViewInDataTable(objGridViewSelecionadas, false,tipoGeracaoCTePorNota);
            MontarEstruturaDataTable(dtReturn, tipoGeracaoCTePorNota, false);
            dtReturn = MontarDisponeisNaoSelecionados(dtReturn, dtSelecionadas, dtDisponiveis, ChaveDocumento);
        }
        else
        {
            string ChaveDocumento = "ChaveCTe";
            //Todas os ctes no grid de Selecionadas
            DataTable dtSelecionadas = TransformarCTeTerceiroGridViewInDataTable(objGridViewSelecionadas, false, tipoGeracaoCTePorCte);
            //Todas os ctes disponiveis que nao foram checadas
            MontarEstruturaDataTable(dtReturn, false, tipoGeracaoCTePorCte);
            //Todos os ctes disponives não selecionados
            dtReturn = MontarDisponeisNaoSelecionados(dtReturn, dtSelecionadas, dtDisponiveis, ChaveDocumento);

        }

        return dtReturn;

    }

    public static DataTable MontarDisponeisNaoSelecionados(DataTable dtReturn, DataTable dtSelecionadas, DataTable dtDisponiveis, string ChaveDocumento)
    {
        try
        {

            //Todos os Documentos disponives não selecionados   
            for (int j = 0; dtSelecionadas.Rows.Count > j; j++)
            {
                DataRow dr = dtSelecionadas.Rows[j];

                Filial objFilial = new Filial();

                objFilial.GetById(Convert.ToInt32(dr["IdFilial"]));

                for (int i = 0; i < dtDisponiveis.Rows.Count; i++)
                {
                    DataRow drDisponivel = dtDisponiveis.Rows[i];
                    bool DocumentoCompativel = true;

                    ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
                    objConfiguraModulo.Get();


                    if (dr[ChaveDocumento].ToString() == drDisponivel[ChaveDocumento].ToString())
                        DocumentoCompativel = false;


                    if (DocumentoCompativel)
                    {

                        if (dtReturn.Rows.Count > 0)
                        {
                            bool NaoExistenteDocumento = true;

                            for (int k = 0; dtReturn.Rows.Count > k; k++)
                            {
                                DataRow drExistente = dtReturn.Rows[k];

                                if (drExistente[ChaveDocumento].ToString() == drDisponivel[ChaveDocumento].ToString())
                                {
                                    NaoExistenteDocumento = false;
                                    continue;
                                }
                            }
                            if (NaoExistenteDocumento)
                            {
                                DataRow drCompativel = dtReturn.NewRow();
                                ClonarDataRow(drDisponivel, drCompativel);
                                dtReturn.Rows.Add(drCompativel);
                            }

                        }
                        else
                        {
                            DataRow drCompativel = dtReturn.NewRow();
                            ClonarDataRow(drDisponivel, drCompativel);
                            dtReturn.Rows.Add(drCompativel);
                        }

                    }
                }
            }

            dtReturn = RemovendoSelecionadosDoDisponiveis(dtReturn, dtSelecionadas, ChaveDocumento);

            return dtReturn;
        }
        finally
        {

            dtDisponiveis.Clear();
            dtSelecionadas.Clear();


            dtDisponiveis.Dispose();
            dtSelecionadas.Dispose();
        }
    }

    public static DataTable RemovendoSelecionadosDoDisponiveis(DataTable dtReturn, DataTable dtSelecionadas, string ChaveDocumento)
    {
        try
        {
            if (dtReturn.Rows.Count == 0)
                return dtSelecionadas;


            //Verificando e removendo do disponiveis algum Documento que ja foi selecionado

            for (int k = 0; dtSelecionadas.Rows.Count > k; k++)
            {
                DataRow drselecionadas = dtSelecionadas.Rows[k];

                for (int t = 0; t < dtReturn.Rows.Count; t++)
                {
                    DataRow drExistente = dtReturn.Rows[t];

                    if (drselecionadas[ChaveDocumento].ToString() == drExistente[ChaveDocumento].ToString())
                    {
                        dtReturn.Rows.Remove(drExistente);
                        t--;
                    }
                }
            }

            return dtReturn;
        }
        finally
        {

            dtSelecionadas.Clear();


            dtSelecionadas.Dispose();
        }
    }


    #region[Metodos staticos Notafiscal Terceiro]
    public static void CarregarNotasDisponiveisBD(GridView objGridViewDisponiveis, GridView objGridViewSelecionadas, string tipoConsulta, ClassSQLCarga.stParametros objparametros, int _NumMaxRegistrosConsulta)
    {
        ClassSQLCarga objClassSQLCarga = new ClassSQLCarga();
        DataTable dtNotas = objClassSQLCarga.ListarNotasFiscaisDisponiveisParaDocFrete(tipoConsulta, objparametros, _NumMaxRegistrosConsulta).Tables[0];

        //Retira de notas disponiveis as Notas no que estao no grid de Selecionadas
        DataTable dtTodasSelecionadas = TransformarGridViewInDataTable(objGridViewSelecionadas, false);
        string _notafiscal = "";
        if (dtTodasSelecionadas.Rows.Count > 0)
        {
            if(dtNotas.Rows.Count > 0)
            {
                for (int i = dtNotas.Rows.Count - 1; i >= 0; i--)
                {
                    _notafiscal = string.IsNullOrEmpty(_notafiscal) ? dtNotas.Rows[i]["IdNotaFiscal"].ToString() : _notafiscal + "," + dtNotas.Rows[i]["IdNotaFiscal"].ToString();
                }
                dtNotas.Select("IdNotaFiscal NOT IN ( " + _notafiscal + " )");
            }
            
        }



        objGridViewDisponiveis.DataSource = dtNotas;
        objGridViewDisponiveis.DataBind();
        dtNotas.Dispose();
        dtTodasSelecionadas.Dispose();
    }

    public static void MigrarDisponiveisChecadasParaSelecionadas(GridView objGridViewDisponiveis, GridView objGridViewSelecionadas, bool tipoGeracaoCTePorNota)
    {
        //Todas as Notas no grid de Disponiveis
        DataTable dtTodasDisponiveis = TransformarGridViewInDataTable(objGridViewDisponiveis, false, tipoGeracaoCTePorNota);
        //Todas as Notas no grid de Selecionadas
        DataTable dtSelecionadas = TransformarGridViewInDataTable(objGridViewSelecionadas, false, tipoGeracaoCTePorNota);
        //Todas as Notas disponiveis que nao foram checadas
        DataTable dtDisponiveisNaoChecada = new DataTable();
        MontarEstruturaDataTable(dtDisponiveisNaoChecada, tipoGeracaoCTePorNota, false);

        try
        {
            for (int i = 0; i < dtTodasDisponiveis.Rows.Count; i++)
            {
                DataRow dr = dtTodasDisponiveis.Rows[i];

                if (dr["Checked"].ToString() == "1") //Checada incluo nas selecionadas
                {
                    DataRow drAdd = dtSelecionadas.NewRow();
                    ClonarDataRow(dr, drAdd);
                    dtSelecionadas.Rows.Add(drAdd);

                }
                else //nao-checada incluo nas disponiveis
                {
                    DataRow drAdd = dtDisponiveisNaoChecada.NewRow();
                    ClonarDataRow(dr, drAdd);
                    dtDisponiveisNaoChecada.Rows.Add(drAdd);
                }
            }

            if (tipoGeracaoCTePorNota)
                validarNotasFiscaisSelecionadas(dtSelecionadas);

            objGridViewDisponiveis.DataSource = FiltrarDisponiveisComSelecionadas(dtDisponiveisNaoChecada, dtSelecionadas, tipoGeracaoCTePorNota);
            objGridViewDisponiveis.DataBind();

            objGridViewSelecionadas.DataSource = dtSelecionadas;
            objGridViewSelecionadas.DataBind();
        }
        finally
        {
            dtTodasDisponiveis.Clear();
            dtDisponiveisNaoChecada.Clear();
            dtSelecionadas.Clear();

            dtTodasDisponiveis.Dispose();
            dtDisponiveisNaoChecada.Dispose();
            dtSelecionadas.Dispose();
        }
    }

    public static void MigrarSelecionadasChecadasParaDisponiveis(GridView objGridViewSelecionadas, GridView objGridViewDisponiveis, string tipoConsulta, ClassSQLCarga.stParametros objparametros, int _NumMaxRegistrosConsulta, bool tipoGeracaoCTePorNota)
    {
        //Todas as Notas no grid de Selecionadas
        DataTable dtTodasSelecionadas = TransformarGridViewInDataTable(objGridViewSelecionadas, false, tipoGeracaoCTePorNota);
        //Todas as Notas no grid de Disponiveis
        DataTable dtDisponiveis = TransformarGridViewInDataTable(objGridViewDisponiveis, false, tipoGeracaoCTePorNota);
        //Todas as Notas Selecionadas que nao foram checadas
        DataTable dtSelecionadasNaoChecada = new DataTable();
        MontarEstruturaDataTable(dtSelecionadasNaoChecada, tipoGeracaoCTePorNota, false);

        try
        {
            for (int i = 0; i < dtTodasSelecionadas.Rows.Count; i++)
            {
                DataRow dr = dtTodasSelecionadas.Rows[i];

                if (dr["Checked"].ToString() == "1") //Checada incluo nas selecionadas
                {
                    DataRow drAdd = dtDisponiveis.NewRow();
                    ClonarDataRow(dr, drAdd);
                    dtDisponiveis.Rows.Add(drAdd);
                }
                else //nao-checada incluo nas selecionadas
                {
                    DataRow drAdd = dtSelecionadasNaoChecada.NewRow();
                    ClonarDataRow(dr, drAdd);
                    dtSelecionadasNaoChecada.Rows.Add(drAdd);
                }
            }

            objGridViewSelecionadas.DataSource = dtSelecionadasNaoChecada;
            objGridViewSelecionadas.DataBind();

            if (dtSelecionadasNaoChecada.Rows.Count > 0)
            {
                objGridViewDisponiveis.DataSource = dtDisponiveis;
                objGridViewDisponiveis.DataBind();
            }
            else
            {
                CarregarNotasDisponiveisBD(objGridViewDisponiveis, objGridViewSelecionadas, tipoConsulta, objparametros, _NumMaxRegistrosConsulta);
            }
        }
        finally
        {
            dtTodasSelecionadas.Clear();
            dtDisponiveis.Clear();
            dtSelecionadasNaoChecada.Clear();

            dtTodasSelecionadas.Dispose();
            dtDisponiveis.Dispose();
            dtSelecionadasNaoChecada.Dispose();
        }
    }

    public static NotaFiscalList getNotaFiscalList(GridView objGridView)
    {
        NotaFiscalList objNotaFiscalList = new NotaFiscalList();

        for (int i = 0; i < objGridView.Rows.Count; i++)
        {
            GridViewRow gvr = objGridView.Rows[i];
            int idNotaFiscal = Convert.ToInt32(((Label)gvr.FindControl("IdNotaFiscal")).Text);
            NotaFiscal objNotaFiscal = new NotaFiscal();
            objNotaFiscal.GetById(idNotaFiscal);
            objNotaFiscalList.Add(objNotaFiscal);
        }

        return objNotaFiscalList;
    }

    public static DataTable TransformarGridViewInDataTable(GridView objGridView, bool apenasSelecionadas)
    {
        DataTable dt = TransformarGridViewInDataTable(objGridView, apenasSelecionadas, true);
        return dt;

    }

    public static DataTable TransformarGridViewInDataTable(GridView objGridView, bool apenasSelecionadas, bool tipoGeracaoCTePorNota)
    {
        DataTable dt = new DataTable();
        MontarEstruturaDataTable(dt, tipoGeracaoCTePorNota, false);

        for (int i = 0; i < objGridView.Rows.Count; i++)
        {
            GridViewRow gvr = objGridView.Rows[i];
            CheckBox chkSelecionado = (CheckBox)gvr.FindControl("chkselect");
            if ((chkSelecionado.Checked) || (!apenasSelecionadas))
            {
                DataRow dr = dt.NewRow();
                if (tipoGeracaoCTePorNota)
                {
                    dr["Checked"] = chkSelecionado.Checked ? "1" : "0";
                    dr["RemetenteCNPJCPF"] = ((Label)gvr.FindControl("RemetenteCNPJCPF")).Text;
                    dr["DestinatarioCnpjCpf"] = ((Label)gvr.FindControl("DestinatarioCnpjCpf")).Text;
                    dr["IdNotaFiscal"] = ((Label)gvr.FindControl("IdNotaFiscal")).Text;
                    dr["NumeroNF"] = ((Label)gvr.FindControl("NumeroNF")).Text;
                    dr["SerieNF"] = ((Label)gvr.FindControl("SerieNF")).Text;
                    dr["TotalNF"] = ((Label)gvr.FindControl("TotalNF")).Text;
                    dr["Remetente"] = ((Label)gvr.FindControl("Remetente")).Text;
                    dr["Destinatario"] = ((Label)gvr.FindControl("Destinatario")).Text;
                    dr["CifFob"] = ((Label)gvr.FindControl("CifFob")).Text;
                    dr["Filial"] = ((Label)gvr.FindControl("Filial")).Text;
                    dr["IdLocalColeta"] = ((Label)gvr.FindControl("IdLocalColeta")).Text;
                    dr["IdLocalEntrega"] = ((Label)gvr.FindControl("IdLocalEntrega")).Text;
                    dr["IdFilial"] = ((Label)gvr.FindControl("IdFilial")).Text;
                    dr["LocalColeta"] = ((Label)gvr.FindControl("Coleta")).Text;
                    dr["LocalEntrega"] = ((Label)gvr.FindControl("Entrega")).Text;
                    dr["VolumeTotal"] = ((Label)gvr.FindControl("Volume")).Text;
                    dr["PesoTotal"] = ((Label)gvr.FindControl("Peso")).Text;
                    dr["FormatoCasasVolume"] = ((Label)gvr.FindControl("FormatoCasasVolume")).Text;
                    dr["FormatoCasasPeso"] = ((Label)gvr.FindControl("FormatoCasasPeso")).Text;
                    dr["DataEmissao"] = ((Label)gvr.FindControl("DataEmissao")).Text;
                    dr["DescNatureza"] = ((Label)gvr.FindControl("DescNatureza")).Text;
                    dr["DescEspecie"] = ((Label)gvr.FindControl("DescEspecie")).Text;
                    dr["ChaveEletronicaNFe"] = ((Label)gvr.FindControl("ChaveEletronicaNFe")).Text;
                    dr["PesoCubado"] = ((Label)gvr.FindControl("PesoCubado")).Text;
                    dt.Rows.Add(dr);
                }

            }
        }
        return dt;
    }
    #endregion

    #region[Metodos staticos Cte Terceiro]
    public static void CarregarConhecimentoTerceiroDisponiveisBD(GridView objGridViewDisponiveis, GridView objGridViewSelecionadas, string tipoConsulta, ClassSQLCarga.stParametrosCTeTerceiro objparametros, int _NumMaxRegistrosConsulta)
    {
        ClassSQLCarga objClassSQLCarga = new ClassSQLCarga();
        DataTable dtConhecimentoTerceiro = objClassSQLCarga.ListarConhecimentoTerceiroDisponiveisParaDocFrete(tipoConsulta, objparametros, _NumMaxRegistrosConsulta).Tables[0];

        //Retira de notas disponiveis as Notas no que estao no grid de Selecionadas
        DataTable dtTodasSelecionadas = TransformarCTeTerceiroGridViewInDataTable(objGridViewSelecionadas, false);

        /* DataTable dtConhecimentoTerceiro = ValidarDisponiveisSelecionavesi(dtDisponiveis, objGridViewSelecionadas, true);*/

        string _ConhecimentoTerceiro = "";
        if (dtTodasSelecionadas.Rows.Count > 0)
        {
            if (dtConhecimentoTerceiro.Rows.Count > 0)
            {
                for (int i = dtConhecimentoTerceiro.Rows.Count - 1; i >= 0; i--)
                {
                    _ConhecimentoTerceiro = string.IsNullOrEmpty(_ConhecimentoTerceiro) ? dtConhecimentoTerceiro.Rows[i]["IdConhecimentoTerceiro"].ToString() : _ConhecimentoTerceiro + "," + dtConhecimentoTerceiro.Rows[i]["IdConhecimentoTerceiro"].ToString();
                }
                dtConhecimentoTerceiro.Select("IdConhecimentoTerceiro NOT IN ( " + _ConhecimentoTerceiro + " )");
            }
                
        }

        objGridViewDisponiveis.DataSource = dtConhecimentoTerceiro;
        objGridViewDisponiveis.DataBind();
        dtConhecimentoTerceiro.Dispose();
        dtTodasSelecionadas.Dispose();
    }
    
    public static void MigrarCteTerceiroDisponiveisChecadasParaSelecionadas(GridView objGridViewDisponiveis, GridView objGridViewSelecionadas, bool tipoGeracaoCTePorCte)
    {
        //Todas as Notas no grid de Disponiveis
        DataTable dtTodasDisponiveis = TransformarCTeTerceiroGridViewInDataTable(objGridViewDisponiveis, false, tipoGeracaoCTePorCte);
        //Todas as Notas no grid de Selecionadas
        DataTable dtSelecionadas = TransformarCTeTerceiroGridViewInDataTable(objGridViewSelecionadas, false, tipoGeracaoCTePorCte);
        //Todas as Notas disponiveis que nao foram checadas
        DataTable dtDisponiveisNaoChecada = new DataTable();
        MontarEstruturaDataTable(dtDisponiveisNaoChecada, false, tipoGeracaoCTePorCte);

        try
        {
            for (int i = 0; i < dtTodasDisponiveis.Rows.Count; i++)
            {
                DataRow dr = dtTodasDisponiveis.Rows[i];

                if (dr["Checked"].ToString() == "1") //Checada incluo nas selecionadas
                {
                    DataRow drAdd = dtSelecionadas.NewRow();
                    ClonarDataRow(dr, drAdd);
                    dtSelecionadas.Rows.Add(drAdd);

                }
                else //nao-checada incluo nas disponiveis
                {
                    DataRow drAdd = dtDisponiveisNaoChecada.NewRow();
                    ClonarDataRow(dr, drAdd);
                    dtDisponiveisNaoChecada.Rows.Add(drAdd);
                }
            }

            if (tipoGeracaoCTePorCte)
                ValidaCteTerceirosSelecionadas(dtSelecionadas);

            objGridViewDisponiveis.DataSource = FiltrarCteTerceiroDisponiveisComSelecionadas(dtDisponiveisNaoChecada, dtSelecionadas, tipoGeracaoCTePorCte);
            objGridViewDisponiveis.DataBind();

            objGridViewSelecionadas.DataSource = dtSelecionadas;
            objGridViewSelecionadas.DataBind();
        }
        finally
        {
            dtTodasDisponiveis.Clear();
            dtDisponiveisNaoChecada.Clear();
            dtSelecionadas.Clear();

            dtTodasDisponiveis.Dispose();
            dtDisponiveisNaoChecada.Dispose();
            dtSelecionadas.Dispose();
        }
    }

    public static void MigrarCteTerceiroSelecionadasChecadasParaDisponiveis(GridView objGridViewSelecionadas, GridView objGridViewDisponiveis, string tipoConsulta, ClassSQLCarga.stParametrosCTeTerceiro objparametros, int _NumMaxRegistrosConsulta, bool tipoGeracaoCTePorCte)
    {
        //Todas as Notas no grid de Selecionadas
        DataTable dtTodasSelecionadas = TransformarCTeTerceiroGridViewInDataTable(objGridViewSelecionadas, false, tipoGeracaoCTePorCte);
        //Todas as Notas no grid de Disponiveis
        DataTable dtDisponiveis = TransformarCTeTerceiroGridViewInDataTable(objGridViewDisponiveis, false, tipoGeracaoCTePorCte);
        //Todas as Notas Selecionadas que nao foram checadas
        DataTable dtSelecionadasNaoChecada = new DataTable();
        MontarEstruturaDataTable(dtSelecionadasNaoChecada, false,tipoGeracaoCTePorCte);

        try
        {
            for (int i = 0; i < dtTodasSelecionadas.Rows.Count; i++)
            {
                DataRow dr = dtTodasSelecionadas.Rows[i];

                if (dr["Checked"].ToString() == "1") //Checada incluo nas selecionadas
                {
                    DataRow drAdd = dtDisponiveis.NewRow();
                    ClonarDataRow(dr, drAdd);
                    dtDisponiveis.Rows.Add(drAdd);
                }
                else //nao-checada incluo nas selecionadas
                {
                    DataRow drAdd = dtSelecionadasNaoChecada.NewRow();
                    ClonarDataRow(dr, drAdd);
                    dtSelecionadasNaoChecada.Rows.Add(drAdd);
                }
            }

            objGridViewSelecionadas.DataSource = dtSelecionadasNaoChecada;
            objGridViewSelecionadas.DataBind();

            if (dtSelecionadasNaoChecada.Rows.Count > 0)
            {
                objGridViewDisponiveis.DataSource = dtDisponiveis;
                objGridViewDisponiveis.DataBind();
            }
            else
            {
                CarregarConhecimentoTerceiroDisponiveisBD(objGridViewDisponiveis, objGridViewSelecionadas, tipoConsulta, objparametros, _NumMaxRegistrosConsulta);
            }
        }
        finally
        {
            dtTodasSelecionadas.Clear();
            dtDisponiveis.Clear();
            dtSelecionadasNaoChecada.Clear();

            dtTodasSelecionadas.Dispose();
            dtDisponiveis.Dispose();
            dtSelecionadasNaoChecada.Dispose();
        }
    }

    public static ConhecimentoTerceiroList GetConhecimentoTerceiroList(GridView objGridView)
    {
        ConhecimentoTerceiroList objConhecimentoTerceiroList = new ConhecimentoTerceiroList();

        for (int i = 0; i < objGridView.Rows.Count; i++)
        {
            GridViewRow gvr = objGridView.Rows[i];
            int idConhecimentoTerceiro = Convert.ToInt32(((Label)gvr.FindControl("IdConhecimentoTerceiro")).Text);
            ConhecimentoTerceiro objConhecimentoTerceiro = new ConhecimentoTerceiro();
            objConhecimentoTerceiro.GetById(idConhecimentoTerceiro);
            objConhecimentoTerceiroList.Add(objConhecimentoTerceiro);
        }

        return objConhecimentoTerceiroList;
    }

    public static DataTable TransformarCTeTerceiroGridViewInDataTable(GridView objGridView, bool apenasSelecionadas)
    {
        DataTable dt = TransformarCTeTerceiroGridViewInDataTable(objGridView, apenasSelecionadas, true);
        return dt;

    }

    public static DataTable TransformarCTeTerceiroGridViewInDataTable(GridView objGridView, bool apenasSelecionadas, bool tipoGeracaoCTePorCte)
    {
        DataTable dt = new DataTable();
        MontarEstruturaDataTable(dt, false, tipoGeracaoCTePorCte);

        for (int i = 0; i < objGridView.Rows.Count; i++)
        {
            GridViewRow gvr = objGridView.Rows[i];
            CheckBox chkSelecionado = (CheckBox)gvr.FindControl("chkselect");
            if ((chkSelecionado.Checked) || (!apenasSelecionadas))
            {
                DataRow dr = dt.NewRow();
                if (tipoGeracaoCTePorCte)
                {
                    dr["Checked"] = chkSelecionado.Checked ? "1" : "0";
                    dr["RemetenteCNPJCPF"] = ((Label)gvr.FindControl("RemetenteCNPJCPF")).Text;
                    dr["DestinatarioCnpjCpf"] = ((Label)gvr.FindControl("DestinatarioCnpjCpf")).Text;
                    dr["EmitenteCnpjCpf"] = ((Label)gvr.FindControl("EmitenteCnpjCpf")).Text;
                    dr["IdConhecimentoTerceiro"] = ((Label)gvr.FindControl("IdConhecimentoTerceiro")).Text;
                    dr["NumCTRC"] = ((Label)gvr.FindControl("NumCTRC")).Text;
                    dr["SerieCTRC"] = ((Label)gvr.FindControl("SerieCTRC")).Text;
                    dr["TotalValorNF"] = ((Label)gvr.FindControl("TotalValorNF")).Text;
                    dr["Remetente"] = ((Label)gvr.FindControl("Remetente")).Text;
                    dr["Destinatario"] = ((Label)gvr.FindControl("Destinatario")).Text;
                    dr["Emitente"] = ((Label)gvr.FindControl("Emitente")).Text;
                    dr["Filial"] = ((Label)gvr.FindControl("Filial")).Text;
                    dr["IdColeta"] = ((Label)gvr.FindControl("IdColeta")).Text;
                    dr["IdEntrega"] = ((Label)gvr.FindControl("IdEntrega")).Text;
                    dr["IdFilial"] = ((Label)gvr.FindControl("IdFilial")).Text;
                    dr["LocalColeta"] = ((Label)gvr.FindControl("Coleta")).Text;
                    dr["LocalEntrega"] = ((Label)gvr.FindControl("Entrega")).Text;
                    dr["TotalPesoNF"] = ((Label)gvr.FindControl("TotalPesoNF")).Text;
                    dr["FormatoCasasPeso"] = ((Label)gvr.FindControl("FormatoCasasPeso")).Text;
                    dr["DataEmissao"] = ((Label)gvr.FindControl("DataEmissao")).Text;
                    dr["DescNatureza"] = ((Label)gvr.FindControl("DescNatureza")).Text;
                    dr["DescEspecie"] = ((Label)gvr.FindControl("DescEspecie")).Text;
                    dr["ChaveCTe"] = ((Label)gvr.FindControl("ChaveCTe")).Text;

                    dt.Rows.Add(dr);
                }
            }
        }
        return dt;
    }
    #endregion

    #endregion


    #region [ Private ]

    #region[Private Nota Terceiro]
    private static void validarNotasFiscaisSelecionadas(DataTable dtSelecionadas)
    {
        ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
        objConfiguraModulo.Get();
        bool validarRemetenteNotasPorCnpj =
            (objConfiguraModulo.CadastroOtimizadoValidarRemetenteNotasPorCnpj == (byte)eSimNao.Sim);
        bool validarRaizCnpjRemetente =
            (objConfiguraModulo.NoCadastroOtimizadoValidarRemetenteNotasPorRaizCnpj == (byte)eSimNao.Sim);

        Filial objFilial = new Filial();

        if (dtSelecionadas.Rows.Count == 0)
            return;

        DataRow dr = dtSelecionadas.Rows[0];

        objFilial.GetById(Convert.ToInt32(dr["IdFilial"]));

        for (int i = 0; i < dtSelecionadas.Rows.Count; i++)
        {
            DataRow drInterna = dtSelecionadas.Rows[i];

            if (objConfiguraModulo.NoCadastroOtimizadoPermitirRelacionarVariasNFsGerarVariosCTes == (byte)eSimNao.Sim && dr["Filial"].ToString() == drInterna["Filial"].ToString())
                continue;

            if (objFilial.ProdutorRural == (byte)eSimNao.Sim)
            {
                throw new Exception("Produtor Rural não é habilitado para emissão de CTe. Verifique o cadastro da NF ou selecione outra NF.");
            }

            if (validarRemetenteNotasPorCnpj)
            {
                if (dr["RemetenteCNPJCPF"].ToString() != drInterna["RemetenteCNPJCPF"].ToString())
                    throw new Exception("As notas selecionadas devem possuir o mesmo CNPJ/CPF no remetente");

            }
            else if (validarRaizCnpjRemetente)
            {
                ServiceLogic.Tool.Tools _objTools = new ServiceLogic.Tool.Tools();
                string cnpjRemetenteA =
                    _objTools.RetiraAlfas(_objTools.FieldString(dr["RemetenteCNPJCPF"]).Trim());
                string cnpjRemetenteB =
                    _objTools.RetiraAlfas(_objTools.FieldString(drInterna["RemetenteCNPJCPF"]).Trim());

                if (cnpjRemetenteA.Length < 8)
                    throw new Exception("A Raiz de CNPJ do remetente é inválida!");
                if (cnpjRemetenteB.Length < 8)
                    throw new Exception("A Raiz de CNPJ do remetente é inválida!");

                cnpjRemetenteA = _objTools.SubString(cnpjRemetenteA, 0, 8);
                cnpjRemetenteB = _objTools.SubString(cnpjRemetenteB, 0, 8);

                if (cnpjRemetenteA != cnpjRemetenteB)
                    throw new Exception("As notas selecionadas devem possuir remetente com a mesma Raiz de CNPJ/CPF!");
            }
            else if (dr["Remetente"].ToString() != drInterna["Remetente"].ToString())
                throw new Exception("As notas selecionadas devem possuir o mesmo remetente");

            if (dr["Filial"].ToString() != drInterna["Filial"].ToString())
                throw new Exception("As notas selecionadas devem possuir a mesma filial!");
            if (objFilial.NaEmissaoCTePermitirVincularNFeDeLocalEntregaDistinto != (byte)eSimNao.Sim)
            {
                if (dr["Destinatario"].ToString() != drInterna["Destinatario"].ToString())
                    throw new Exception("As notas selecionadas devem possuir o mesmo destinatario");

                if (dr["LocalColeta"].ToString() != drInterna["LocalColeta"].ToString())
                    throw new Exception("As notas selecionadas devem possuir o mesmo local de coleta");

                if (dr["LocalEntrega"].ToString() != drInterna["LocalEntrega"].ToString())
                    throw new Exception("As notas selecionadas devem possuir o mesmo local de entrega");
            }

            if (objConfiguraModulo.ConsiderarCifFobOriundoNotaFiscalCadastroOtimizadoCTE == (byte)eSimNao.Sim)
            {
                if (drInterna["CifFob"].ToString() == "0" || drInterna["CifFob"].ToString() == "1")
                {
                    drInterna["CifFob"] = "C";
                }

                if (dr["CifFob"].ToString() != drInterna["CifFob"].ToString())
                    throw new Exception("As notas selecionadas devem possuir a mesma caracteristicas de pagamento (Cif/Fob)");
            }
        }
    }

    private static DataTable FiltrarDisponiveisComSelecionadas(DataTable dtDisponiveis, DataTable dtSelecionadas, bool tipoGeracaoCTePorNota)
    {
        if (dtSelecionadas.Rows.Count == 0)
            return dtDisponiveis;

        if (dtDisponiveis.Rows.Count == 0)
            return dtDisponiveis;

        DataRow dr = dtSelecionadas.Rows[0];

        Filial objFilial = new Filial();

        objFilial.GetById(Convert.ToInt32(dr["IdFilial"]));

        DataTable dtReturn = new DataTable();
        MontarEstruturaDataTable(dtReturn, tipoGeracaoCTePorNota, false);


        for (int i = 0; i < dtDisponiveis.Rows.Count; i++)
        {
            DataRow drDisponivel = dtDisponiveis.Rows[i];
            bool notaCompativel = true;

            ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
            objConfiguraModulo.Get();

            if (tipoGeracaoCTePorNota)
            {
                if (objConfiguraModulo.ConsiderarFiltroDisponiveisCNPJRemetenteDestinatarioInvesDaRazaoSocial == (byte)eSimNao.Sim)
                {
                    if (dr["RemetenteCNPJCPF"].ToString() != drDisponivel["RemetenteCNPJCPF"].ToString())
                        notaCompativel = false;

                    if (dr["DestinatarioCnpjCpf"].ToString() != drDisponivel["DestinatarioCnpjCpf"].ToString())
                        notaCompativel = false;
                }
                else
                {
                    if (dr["Remetente"].ToString() != drDisponivel["Remetente"].ToString())
                        notaCompativel = false;

                }

                //Apenas a Filial é necessária com o parâmetro: NoCadastroOtimizadoPermitirRelacionarVariasNFsGerarVariosCTes
                if (objConfiguraModulo.NoCadastroOtimizadoPermitirRelacionarVariasNFsGerarVariosCTes == (byte)eSimNao.Sim && dr["Filial"].ToString() == drDisponivel["Filial"].ToString())
                {
                    notaCompativel = true;
                }
                else if (dr["Filial"].ToString() != drDisponivel["Filial"].ToString())
                    notaCompativel = false;
            }

            if (objFilial.NaEmissaoCTePermitirVincularNFeDeLocalEntregaDistinto == (byte)eSimNao.Nao)
            {
                if (dr["Destinatario"].ToString() != drDisponivel["Destinatario"].ToString())
                    notaCompativel = false;

                if (dr["LocalColeta"].ToString() != drDisponivel["LocalColeta"].ToString())
                    notaCompativel = false;

                if (dr["LocalEntrega"].ToString() != drDisponivel["LocalEntrega"].ToString())
                    notaCompativel = false;
            }

            if (notaCompativel)
            {
                DataRow drCompativel = dtReturn.NewRow();
                ClonarDataRow(drDisponivel, drCompativel);
                dtReturn.Rows.Add(drCompativel);
            }
        }

        return dtReturn;
    }

    private static void ClonarDataRow(DataRow drOrigem, DataRow drDestino)
    {
        for (int i = 0; i < drOrigem.Table.Columns.Count; i++)
        {
            drDestino[drOrigem.Table.Columns[i].ColumnName] = drOrigem[drOrigem.Table.Columns[i].ColumnName];
        }
    }

    private static void MontarEstruturaDataTable(DataTable dt, bool tipoGeracaoCTePorNota, bool tipoGeracaoCTePorCte)
    {
        if (tipoGeracaoCTePorNota)
        {
            dt.Columns.Clear();
            dt.Columns.Add("Checked", typeof(string));
            dt.Columns.Add("DestinatarioCnpjCpf", typeof(string));
            dt.Columns.Add("RemetenteCNPJCPF", typeof(string));
            dt.Columns.Add("IdNotaFiscal", typeof(string));
            dt.Columns.Add("NumeroNF", typeof(string));
            dt.Columns.Add("SerieNF", typeof(string));
            dt.Columns.Add("TotalNF", typeof(string));
            dt.Columns.Add("Remetente", typeof(string));
            dt.Columns.Add("Destinatario", typeof(string));
            dt.Columns.Add("CifFob", typeof(string));
            dt.Columns.Add("Filial", typeof(string));
            dt.Columns.Add("IdLocalColeta", typeof(string));
            dt.Columns.Add("IdLocalEntrega", typeof(string));
            dt.Columns.Add("IdFilial", typeof(string));
            dt.Columns.Add("LocalColeta", typeof(string));
            dt.Columns.Add("LocalEntrega", typeof(string));
            dt.Columns.Add("VolumeTotal", typeof(string));
            dt.Columns.Add("PesoTotal", typeof(string));
            dt.Columns.Add("FormatoCasasVolume", typeof(string));
            dt.Columns.Add("FormatoCasasPeso", typeof(string));
            dt.Columns.Add("DataEmissao", typeof(string));
            dt.Columns.Add("DescNatureza", typeof(string));
            dt.Columns.Add("DescEspecie", typeof(string));
            dt.Columns.Add("ChaveEletronicaNFe", typeof(string));
            dt.Columns.Add("PesoCubado", typeof(string));
        }
        else
        {
            dt.Columns.Clear();
            dt.Columns.Add("Checked", typeof(string));
            dt.Columns.Add("DestinatarioCnpjCpf", typeof(string));
            dt.Columns.Add("RemetenteCNPJCPF", typeof(string));
            dt.Columns.Add("EmitenteCNPJCPF", typeof(string));
            dt.Columns.Add("IdConhecimentoTerceiro", typeof(string));
            dt.Columns.Add("NumCTRC", typeof(string));
            dt.Columns.Add("SerieCTRC", typeof(string));
            dt.Columns.Add("TotalValorNF", typeof(string));
            dt.Columns.Add("Remetente", typeof(string));
            dt.Columns.Add("Destinatario", typeof(string));
            dt.Columns.Add("Emitente", typeof(string));
            dt.Columns.Add("Filial", typeof(string));
            dt.Columns.Add("IdColeta", typeof(string));
            dt.Columns.Add("IdEntrega", typeof(string));
            dt.Columns.Add("IdFilial", typeof(string));
            dt.Columns.Add("LocalColeta", typeof(string));
            dt.Columns.Add("LocalEntrega", typeof(string));
            dt.Columns.Add("TotalPesoNF", typeof(string));
            dt.Columns.Add("FormatoCasasPeso", typeof(string));
            dt.Columns.Add("DataEmissao", typeof(string));
            dt.Columns.Add("DescNatureza", typeof(string));
            dt.Columns.Add("DescEspecie", typeof(string));
            dt.Columns.Add("ChaveCTe", typeof(string));
        }

    }
    #endregion

    #region[Private CTE Terceiro]
    // olhar com calmar esse metodo
    private static void ValidaCteTerceirosSelecionadas(DataTable dtSelecionadas)
    {
        ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
        objConfiguraModulo.Get();

        Filial objFilial = new Filial();

        if (dtSelecionadas.Rows.Count == 0)
            return;

        DataRow dr = dtSelecionadas.Rows[0];

        objFilial.GetById(Convert.ToInt32(dr["IdFilial"]));

        for (int i = 0; i < dtSelecionadas.Rows.Count; i++)
        {
            DataRow drInterna = dtSelecionadas.Rows[i];


            if (dr["EmitenteCNPJCPF"].ToString() != drInterna["EmitenteCNPJCPF"].ToString())
                throw new Exception("Os CT-e(s) de Terceiros selecionadas devem possuir o mesmo CNPJ/CPF no Emitente");

            if (dr["Filial"].ToString() != drInterna["Filial"].ToString())
                throw new Exception("Os CT-e(s) de Terceiros selecionadas devem possuir a mesma filial!");


        }
    }

    private static DataTable FiltrarCteTerceiroDisponiveisComSelecionadas(DataTable dtDisponiveis, DataTable dtSelecionadas, bool tipoGeracaoCTePorCte)
    {
        if (dtSelecionadas.Rows.Count == 0)
            return dtDisponiveis;

        if (dtDisponiveis.Rows.Count == 0)
            return dtDisponiveis;

        DataRow dr = dtSelecionadas.Rows[0];

        Filial objFilial = new Filial();

        objFilial.GetById(Convert.ToInt32(dr["IdFilial"]));

        DataTable dtReturn = new DataTable();
        MontarEstruturaDataTable(dtReturn, false, tipoGeracaoCTePorCte);


        for (int i = 0; i < dtDisponiveis.Rows.Count; i++)
        {
            DataRow drDisponivel = dtDisponiveis.Rows[i];
            bool CteTerceiroCompativel = true;

            ConfiguraModulo objConfiguraModulo = new ConfiguraModulo();
            objConfiguraModulo.Get();

            if (tipoGeracaoCTePorCte)
            {
                if (dr["EmitenteCNPJCPF"].ToString() != drDisponivel["EmitenteCNPJCPF"].ToString())
                    CteTerceiroCompativel = false;

                if (dr["ChaveCTe"].ToString() == drDisponivel["ChaveCTe"].ToString())
                    CteTerceiroCompativel = false;


                //Apenas a Filial é necessária com o parâmetro: NoCadastroOtimizadoPermitirRelacionarVariasNFsGerarVariosCTes
                if (objConfiguraModulo.NoCadastroOtimizadoPermitirRelacionarVariasNFsGerarVariosCTes == (byte)eSimNao.Sim && dr["Filial"].ToString() == drDisponivel["Filial"].ToString())
                {
                    CteTerceiroCompativel = true;
                }
                else if (dr["Filial"].ToString() != drDisponivel["Filial"].ToString())
                    CteTerceiroCompativel = false;

                if (CteTerceiroCompativel)
                {
                    DataRow drCompativel = dtReturn.NewRow();
                    ClonarDataRow(drDisponivel, drCompativel);
                    dtReturn.Rows.Add(drCompativel);
                }

            }

        }

        return dtReturn;
    }

    /*private static void montarEstruturaCteterceiroDataTable(DataTable dt, bool tipoGeracaoCTePorCTe)
    {
        if (tipoGeracaoCTePorCTe)
        {         
            dt.Columns.Clear();
            dt.Columns.Add("Checked", typeof(string));
            dt.Columns.Add("DestinatarioCnpjCpf", typeof(string));
            dt.Columns.Add("RemetenteCNPJCPF", typeof(string));
            dt.Columns.Add("EmitenteCNPJCPF", typeof(string));
            dt.Columns.Add("IdConhecimentoTerceiro", typeof(string));
            dt.Columns.Add("NumCTRC", typeof(string));
            dt.Columns.Add("SerieCTRC", typeof(string));
            dt.Columns.Add("TotalValorNF", typeof(string));
            dt.Columns.Add("Remetente", typeof(string));
            dt.Columns.Add("Destinatario", typeof(string));
            dt.Columns.Add("Emitente", typeof(string));
            dt.Columns.Add("Filial", typeof(string));
            dt.Columns.Add("IdColeta", typeof(string));
            dt.Columns.Add("IdEntrega", typeof(string));
            dt.Columns.Add("IdFilial", typeof(string));
            dt.Columns.Add("LocalColeta", typeof(string));
            dt.Columns.Add("LocalEntrega", typeof(string));
            dt.Columns.Add("TotalPesoNF", typeof(string));
            dt.Columns.Add("FormatoCasasPeso", typeof(string));
            dt.Columns.Add("DataEmissao", typeof(string));
            dt.Columns.Add("DescNatureza", typeof(string));
            dt.Columns.Add("DescEspecie", typeof(string));
            dt.Columns.Add("ChaveCTe", typeof(string));
        }
    }*/

    

    #endregion

    #endregion
}
#endregion


