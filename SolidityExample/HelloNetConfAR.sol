pragma solidity ^0.4.24;

contract HelloNetConfAR {
    address private deployer;
    string message;

    constructor() public {
        deployer = msg.sender;
        message = "Give me something to say!";
    }

    function setMessage(string _message) public {
        message = _message;
    }

    function sayMessage() constant returns (string) {
        return message;
    }
    
    function getDeployer() constant returns (address){
        return deployer;
    }
}
