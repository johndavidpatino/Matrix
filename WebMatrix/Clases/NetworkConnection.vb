Imports System.Runtime.InteropServices
Imports System.Net
Imports System.ComponentModel

''' <summary>
''' Represents a network connection along with authentication to a network share.
''' </summary>
Public Class NetworkConnection
    Implements IDisposable
#Region "Variables"

    ''' <summary>
    ''' The full path of the directory.
    ''' </summary>
    Private ReadOnly _networkName As String

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Initializes a new instance of the <see cref="NetworkConnection"/> class.
    ''' </summary>
    ''' <param name="networkName">
    ''' The full path of the network share.
    ''' </param>
    ''' <param name="credentials">
    ''' The credentials to use when connecting to the network share.
    ''' </param>
    Public Sub New(networkName As String, credentials As NetworkCredential)
        _networkName = networkName

        Dim netResource = New NetResource() With { _
             .Scope = ResourceScope.GlobalNetwork, _
             .ResourceType = ResourceType.Disk, _
             .DisplayType = ResourceDisplaytype.Share, _
             .RemoteName = networkName.TrimEnd("\"c) _
        }

        Dim result = WNetAddConnection2(netResource, credentials.Password, credentials.UserName, 0)

        If result <> 0 Then
            Throw New Win32Exception(result)
        End If
    End Sub

#End Region

#Region "Events"

    ''' <summary>
    ''' Occurs when this instance has been disposed.
    ''' </summary>
    Public Event Disposed As EventHandler(Of EventArgs)

#End Region

#Region "Public methods"

    ''' <summary>
    ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "Protected methods"

    ''' <summary>
    ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    ''' </summary>
    ''' <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposing Then
            RaiseEvent Disposed(Me, EventArgs.Empty)
        End If

        WNetCancelConnection2(_networkName, 0, True)
    End Sub

#End Region

#Region "Private static methods"

    ''' <summary>
    '''The WNetAddConnection2 function makes a connection to a network resource. The function can redirect a local device to the network resource.
    ''' </summary>
    ''' <param name="netResource">A <see cref="NetResource"/> structure that specifies details of the proposed connection, such as information about the network resource, the local device, and the network resource provider.</param>
    ''' <param name="password">The password to use when connecting to the network resource.</param>
    ''' <param name="username">The username to use when connecting to the network resource.</param>
    ''' <param name="flags">The flags. See http://msdn.microsoft.com/en-us/library/aa385413%28VS.85%29.aspx for more information.</param>
    ''' <returns></returns>
    <DllImport("mpr.dll")> _
    Private Shared Function WNetAddConnection2(netResource As NetResource, password As String, username As String, flags As Integer) As Integer
    End Function

    ''' <summary>
    ''' The WNetCancelConnection2 function cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.
    ''' </summary>
    ''' <param name="name">Specifies the name of either the redirected local device or the remote network resource to disconnect from.</param>
    ''' <param name="flags">Connection type. The following values are defined:
    ''' 0: The system does not update information about the connection. If the connection was marked as persistent in the registry, the system continues to restore the connection at the next logon. If the connection was not marked as persistent, the function ignores the setting of the CONNECT_UPDATE_PROFILE flag.
    ''' CONNECT_UPDATE_PROFILE: The system updates the user profile with the information that the connection is no longer a persistent one. The system will not restore this connection during subsequent logon operations. (Disconnecting resources using remote names has no effect on persistent connections.)
    ''' </param>
    ''' <param name="force">Specifies whether the disconnection should occur if there are open files or jobs on the connection. If this parameter is FALSE, the function fails if there are open files or jobs.</param>
    ''' <returns></returns>
    <DllImport("mpr.dll")> _
    Private Shared Function WNetCancelConnection2(name As String, flags As Integer, force As Boolean) As Integer
    End Function

#End Region

    ''' <summary>
    ''' Finalizes an instance of the <see cref="NetworkConnection"/> class.
    ''' Allows an <see cref="System.Object"></see> to attempt to free resources and perform other cleanup operations before the <see cref="System.Object"></see> is reclaimed by garbage collection.
    ''' </summary>
    Protected Overrides Sub Finalize()
        Try
            Dispose(False)
        Finally
            MyBase.Finalize()
        End Try
    End Sub
End Class

#Region "Objects needed for the Win32 functions"

''' <summary>
''' The net resource.
''' </summary>
<StructLayout(LayoutKind.Sequential)> _
Public Class NetResource
    Public Scope As ResourceScope
    Public ResourceType As ResourceType
    Public DisplayType As ResourceDisplaytype
    Public Usage As Integer
    Public LocalName As String
    Public RemoteName As String
    Public Comment As String
    Public Provider As String
End Class

''' <summary>
''' The resource scope.
''' </summary>
Public Enum ResourceScope
    Connected = 1
    GlobalNetwork
    Remembered
    Recent
    Context
End Enum

''' <summary>
''' The resource type.
''' </summary>
Public Enum ResourceType
    Any = 0
    Disk = 1
    Print = 2
    Reserved = 8
End Enum

''' <summary>
''' The resource displaytype.
''' </summary>
Public Enum ResourceDisplaytype
    Generic = &H0
    Domain = &H1
    Server = &H2
    Share = &H3
    File = &H4
    Group = &H5
    Network = &H6
    Root = &H7
    Shareadmin = &H8
    Directory = &H9
    Tree = &HA
    Ndscontainer = &HB
End Enum
#End Region

