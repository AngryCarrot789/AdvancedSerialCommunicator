# AdvancedSerialCommunicator
A multithreading serial communicator app, simply used for communicating through serial ports. isn't very advanced atm but will be soon ;)


Message receiving is done on another thread, and it constantly reads a single char and pushes into a buffer until there's either no data left, or if a new line character is found it classes it as a new message, and then clears the buffer. and it constantly does it. If there's no data, it sleeps for 10ms and tries again until theres more data. Doesnt eat up CPU because Thread.Sleep is very nifty
