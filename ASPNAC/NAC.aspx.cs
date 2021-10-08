using NACBackEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNAC
{
    public partial class NAC : System.Web.UI.Page
    {
        GameConnection conn;
        public ObservableCollection<NodeVM> Nodes => new ObservableCollection<NodeVM>(conn.TheNodes.Select((item) => new NodeVM(item)));
        public ObservableCollection<NodeVM> OptionNodes { get => conn.ActiveNeuralNet == null ? new ObservableCollection<NodeVM>() : new ObservableCollection<NodeVM>(conn.ActiveNeuralNet.CurrentNode.Options.Select((item) => createNodeVM(item))); }

        public List<NetVM> Nets => new List<NetVM>(conn.TheNets.Select((item) => new NetVM(item)));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack )
            {
                conn = new GameConnection();
                Session["Conn"] = conn;
                SetButtons(false);
                StartButton.Enabled = false;
                AIMoveButton.Enabled = false;
            }
            else
            {
                conn = (GameConnection)Session["Conn"];
                if (Session["ActiveNeuralNet"] != null)
                {
                    conn.ActiveNeuralNet = (NeuralNet)Session["ActiveNeuralNet"];

                }
                if (Session["ActiveGame"] != null)
                {
                    conn.ActiveGame = (Game)Session["ActiveGame"];

                }
            }

            if (conn.ActiveGame == null) // No Game created
            {
                StartButton.Enabled = false;
                AIMoveButton.Enabled = false;
                SetButtons(false);
            }
            else if (conn.ActiveGame.GameActive == false) // Game created but not started
            {
                StartButton.Enabled = true;
                AIMoveButton.Enabled = false;
                SetButtons(false);

            }
            else if (conn.ActiveGame.GameWon == false) // Game is running
            {
                StartButton.Enabled = true;
                SetButtons(true);
            }
            else
            {
                StartButton.Enabled = true;
                AIMoveButton.Enabled = false;
                SetButtons(false);
            }
            NetTest.DataSource = Nets;
            NetTest.DataBind();

        }

        private NodeVM createNodeVM(string item)
        {
            if (item == null)
            {
                return new NodeVM();
            }
            else
            {
                return new NodeVM(conn.ActiveNeuralNet.GetNode(new Board(item, null)).SqlNode);
            }

        }

        protected void NetTest_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void NetTest_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            conn.ActiveNet = Nets[e.NewSelectedIndex].TheNet;
            Session["ActiveNeuralNet"] = conn.ActiveNeuralNet;
            NodeView.DataSource = Nodes;
            NodeView.DataBind();
            StartButton.Enabled = true;
        }

        protected void NAC_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            SetButtons(false);
            clicked.Text = conn.ActiveGame.Activeplayer.Marker == SquareState.Nought ? "O" : "X";
            conn.ActiveGame.PlayerMove(GetSquare(clicked.ID));
            AIMoveButton.Enabled = true;
            AIMovesOptionsTable.DataSource = OptionNodes;
            AIMovesOptionsTable.DataBind();

        }

        protected void StartButton_Click(object sender, EventArgs e)
        {
            if (conn.ActiveNet != null)
            {
                conn.StartGame(true);
                Session["ActiveGame"] = conn.ActiveGame;
                SetButtons(true);
                StartButton.Enabled = false;
            }
        }

        private SquareID GetSquare (string ID)
        {
            switch(ID)
            {
                case "TL":
                    return SquareID.TopLeft;
                case "TC":
                    return SquareID.TopCenter;
                case "TR":
                    return SquareID.TopRight;
                case "CL":
                    return SquareID.CenterLeft;
                case "CC":
                    return SquareID.CenterCenter;
                case "CR":
                    return SquareID.CenterRight;
                case "BL":
                    return SquareID.BottomLeft;
                case "BC":
                    return SquareID.BottomCenter;
                default:
                    return SquareID.BottomRight;
            }
        }

        private void SetButtons(bool state)
        {
            TL.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.TopLeft) == SquareState.Blank ? true : false : false;
            TC.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.TopCenter) == SquareState.Blank ? true : false : false;
            TR.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.TopRight) == SquareState.Blank ? true : false : false;
            CL.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.CenterLeft) == SquareState.Blank ? true : false : false;
            CC.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.CenterCenter) == SquareState.Blank ? true : false : false;
            CR.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.CenterRight) == SquareState.Blank ? true : false : false;
            BL.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.BottomLeft) == SquareState.Blank ? true : false : false;
            BC.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.BottomCenter) == SquareState.Blank ? true : false : false;
            BR.Enabled = state ? conn.ActiveGame.CurrentNode.Theboard.getSquareState(SquareID.BottomRight) == SquareState.Blank ? true : false : false;
        }
    }
}