/// bytecode:
/// 0x608060405234801561001057600080fd5b5060038054600160a060020a031916331790556000805460ff1916905561047a8061003c6000396000f30060806040526004361061008d5763ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416631aa3a008811461009257806333b16d93146100a9578063522c669c146100da5780635ec01e4d146100ef578063680c89c2146101165780638da5cb5b1461012e5780639dc36fb314610143578063f60cdcf614610178575b600080fd5b34801561009e57600080fd5b506100a76101a3565b005b3480156100b557600080fd5b506100be61029f565b60408051600160a060020a039092168252519081900360200190f35b3480156100e657600080fd5b506100a76102d6565b3480156100fb57600080fd5b506101046103c8565b60408051918252519081900360200190f35b34801561012257600080fd5b506100be6004356103f9565b34801561013a57600080fd5b506100be610421565b34801561014f57600080fd5b50610164600160a060020a0360043516610430565b604080519115158252519081900360200190f35b34801561018457600080fd5b5061018d610445565b6040805160ff9092168252519081900360200190f35b3360009081526002602052604090205460ff161561022257604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601a60248201527f53656e64657220697320616c7265616479207265676973746572000000000000604482015290519081900360640190fd5b6001805480820182557fb10e2d527612073b26eecdfd717e6a320cf44b4afac2b0732d9fcbe2b7fa0cf601805473ffffffffffffffffffffffffffffffffffffffff1916339081179091556000908152600260205260408120805460ff199081168417909155815460ff8181169094019093169216919091179055565b6000806102aa6103c8565b90506001818154811015156102bb57fe5b600091825260209091200154600160a060020a031691505090565b600354600090600160a060020a0316331461035257604080517f08c379a000000000000000000000000000000000000000000000000000000000815260206004820152601860248201527f53656e646572206973206e6f74206175746f72697a65642e0000000000000000604482015290519081900360640190fd5b5060005b60005460ff90811690821610156103bb5760006002600060018460ff1681548110151561037f57fe5b600091825260208083209190910154600160a060020a031683528201929092526040019020805460ff1916911515919091179055600101610356565b506000805460ff19169055565b6000805460408051428152436020820152815190819003909101902060ff909116908115156103f357fe5b06905090565b600180548290811061040757fe5b600091825260209091200154600160a060020a0316905081565b600354600160a060020a031681565b60026020526000908152604090205460ff1681565b60005460ff16815600a165627a7a72305820d05ffc1d361083f794b74eb4f57c226ee7c9a437a4739b2f8f20a4c968034aa70029
/// abi:
/// [{\"constant\":false,\"inputs\":[],\"name\":\"register\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"determineWinner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[],\"name\":\"restartLotery\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"random\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"playersByNumber\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"address\"}],\"name\":\"isPlayerRegister\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"totalPlayers\",\"outputs\":[{\"name\":\"\",\"type\":\"uint8\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"}]


pragma solidity ^0.4.8;

contract LotteryNetConfAR {
    
    uint8 public totalPlayers;
    address[] public playersByNumber;
    mapping (address => bool ) public isPlayerRegister;
    address public owner;
    
    
    constructor () public {
        owner = msg.sender;
        totalPlayers = 0;
    }
    
    function register() public {
        require(!isPlayerRegister[msg.sender], "Sender is already register");
        playersByNumber.push(msg.sender);
        isPlayerRegister[msg.sender] = true;
        totalPlayers++;
    }
    
    function determineWinner() public view returns(address) {
        uint256 winningNumber = random();
        return playersByNumber[winningNumber];
    }
    
    function random() public view returns (uint256) {
        return uint256(uint256(keccak256(block.timestamp, block.number))%totalPlayers);
    }
    
    function restartLotery() public{
        require(msg.sender == owner, "Sender is not autorized.");
        
        for (uint8 i = 0; i < totalPlayers; i++){
            isPlayerRegister[playersByNumber[i]] = false;
        }
        
        totalPlayers = 0;
    }
    
}