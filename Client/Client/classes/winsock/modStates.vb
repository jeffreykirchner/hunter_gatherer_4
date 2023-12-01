''' <summary>
''' Enumeration containing the various Winsock states.
''' </summary>
Public Enum WinsockStates
    ''' <summary>
    ''' The Winsock is closed.
    ''' </summary>
    Closed = 0
    ''' <summary>
    ''' The Winsock is listening (TCP for connections, UDP for data).
    ''' </summary>
    Listening = 1
    ''' <summary>
    ''' The Winsock is attempting the resolve the remote host.
    ''' </summary>
    ResolvingHost = 2
    ''' <summary>
    ''' The remote host has been resolved to IP address.
    ''' </summary>
    HostResolved = 3
    ''' <summary>
    ''' The Winsock is attempting to connect to the remote host.
    ''' </summary>
    Connecting = 4
    ''' <summary>
    ''' The Winsock is connected to a remote source (client or server).
    ''' </summary>
    Connected = 5
    ''' <summary>
    ''' The Winsock is attempting to close the connection.
    ''' </summary>
    Closing = 6
End Enum

''' <summary>
''' Enumeration containing the various supported network protocols.
''' </summary>
Public Enum WinsockProtocols
    ''' <summary>
    ''' The Winsock should use the connection-based TCP protocol.
    ''' </summary>
    Tcp = 0
    ''' <summary>
    ''' The Winsock should use the connectionless-based UDP protocol.
    ''' </summary>
    Udp = 1
End Enum

''' <summary>
''' Enumeration containing the various supported IP protocols.
''' </summary>
Public Enum WinsockIPTypes
    ''' <summary>
    ''' The Winsock should use the IPv4 addressing scheme
    ''' </summary>
    IPv4 = 0
    ''' <summary>
    ''' The Winsock should use the IPv6 addressing scheme
    ''' </summary>
    IPv6 = 1
End Enum