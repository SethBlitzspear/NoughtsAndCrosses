<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NAC.aspx.cs" Inherits="ASPNAC.NAC" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="CSS/NAC.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="mainpanel">
            <div class="subpanel" id="NetPanel">
                <span class="title">Neural Net</span>

                <asp:ListView ID="NetTest" DataKeyNames="Name" runat="server" OnSelectedIndexChanged="NetTest_SelectedIndexChanged" OnSelectedIndexChanging="NetTest_SelectedIndexChanging" AutoPostBack="true">
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <th runat="server">Net Name</th>
                                <th runat="server">Win</th>
                                <th runat="server">Loss</th>
                                <th runat="server">Draw</th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr runat="server">
                            <td>
                                <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select">
                                    <asp:Label ID="NetName" runat="server" Text='<%# Eval("Name") %>' />
                                </asp:LinkButton></td><td>
                                <asp:Label ID="Win" runat="server" Text='<%# Eval("Win") %>' />
                            </td>
                            <td>
                                <asp:Label ID="Loss" runat="server" Text='<%# Eval("Loss") %>' />
                            </td>
                            <td>
                                <asp:Label ID="Draw" runat="server" Text='<%# Eval("Draw") %>' />
                            </td>

                        </tr>
                    </ItemTemplate>
                </asp:ListView>

            </div>
            <div class="subpanel" id="NodePanel">
                <span class="title">Nodes</span> 
                <asp:ListView ID="NodeView" DataKeyNames="Name" runat="server">
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <th runat="server">Name</th><th runat="server">Weight</th></tr><tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr runat="server">
                            <td>
                                <asp:Label ID="NetName" runat="server" Text='<%# Eval("Name") %>' />
                            </td>
                            <td>
                                <asp:Label ID="Win" runat="server" Text='<%# Eval("Weight") %>' />
                            </td>

                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div id="Gamecolumn">
                <span class="subpanel" id="GameTitle">Noughts and Crosses </span><div class="subpanel" id="GamePanel">
                    <div id="GameDetailsPanel">
                        <span id="GameButtonsPanel">
                            <asp:Button runat="server" ID="StartButton" Text="Start" CssClass="GameControlButtons" OnClick="StartButton_Click" />
                            <asp:Button runat="server" ID="AIMoveButton" Text="AI Move" CssClass="GameControlButtons" />
                        </span>
                        <span id="PlayerDetailsPanel">
                            <span class="PlayerPanels title">
                                <span class="title">Player 1
                                </span>
                                <span class="AIChecks">
                                    <span>A.I.
                                    </span>
                                    <span>
                                        <asp:CheckBox runat="server" ID="Player1AICheckBox" />
                                    </span>
                                </span>
                            </span>
                            <span class="PlayerPanels title">
                                <span class="title">Player 2
                                </span>
                                <span class="AIChecks">
                                    <span>A.I.
                                    </span>
                                    <span>
                                        <asp:CheckBox runat="server" ID="Player2AICheckBox" />
                                    </span>
                                </span>
                            </span>
                            <span class="PlayerPanels title">
                                <span class="title">Game State
                                </span>
                                <span>
                                    <asp:Label runat="server" ID="GameStateString" />
                                </span>
                            </span>
                        </span>

                        <div id="AIMoveOptions" class="AIMovesPanel title">
                            <span class="title">AI Move Options</span>
                            <div class="AIMovesTable">
                                <asp:ListView ID="AIMovesOptionsTable" DataKeyNames="Name" runat="server">
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <th runat="server">Name</th><th runat="server">Weight</th></tr><tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr runat="server">
                            <td>
                                <asp:Label ID="NetName" runat="server" Text='<%# Eval("Name") %>' />
                            </td>
                            <td>
                                <asp:Label ID="Win" runat="server" Text='<%# Eval("Weight") %>' />
                            </td>

                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                            </div>
                        </div>
                        <div id="AIMoveHistory" class="AIMovesPanel title">
                            <span class="title">AI Move History</span>
                            <div class="AIMovesTable">
                            </div>
                        </div>
                    </div>
                    <div id="NACGridRows">
                        <span class="NACGridColumns">
                            <asp:Button ID="TL" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click"/>
                            <asp:Button ID="TC" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click" />
                            <asp:Button ID="TR" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click" />
                        </span>
                        <span class="NACGridColumns">
                            <asp:Button ID="CL" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click" />
                            <asp:Button ID="CC" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click" />
                            <asp:Button ID="CR" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click" />

                        </span>
                        <span class="NACGridColumns">
                            <asp:Button ID="BL" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click" />
                            <asp:Button ID="BC" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click" />
                            <asp:Button ID="BR" CssClass="NACButtons" runat="server" Text="" OnClick="NAC_Click" />

                        </span>

                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
