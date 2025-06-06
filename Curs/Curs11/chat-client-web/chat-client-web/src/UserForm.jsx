
import React from  'react';
import { useState } from 'react';
export default function UserForm({addFunc}){

    const [name, setName] = useState('');
    const [username, setUsername] = useState('');
    const [passwd, setPasswd] = useState('');


   function handleSubmit (event){

        let user={id:username,
            name:name,
            passwd:passwd
        }
        console.log('A user was submitted: ');
        console.log(user);
         addFunc(user);
        event.preventDefault();
    }
    return(
    <form onSubmit={handleSubmit}>
        <label>
            Username:
            <input type="text" value={username} onChange={e=>setUsername(e.target.value)} />
        </label><br/>
        <label>
            Name:
            <input type="text" value={name} onChange={e=>setName(e.target.value)} />
        </label><br/>
        <label>
            Passwd:
            <input type="password" value={passwd} onChange={e=>setPasswd(e.target.value)} />
        </label><br/>

        <input type="submit" value="Add user" />
    </form>);
}
/*class UserForm extends React.Component{

    constructor(props) {
        super(props);
        this.state = {username: '', name:'', passwd:''};

      //  this.handleChange = this.handleChange.bind(this);
       // this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleUserChange=(event) =>{
        this.setState({username: event.target.value});
    }

    handleNameChange=(event) =>{
       this.setState({name: event.target.value});
    }

    handlePasswdChange=(event) =>{
        this.setState({passwd: event.target.value});
    }
    handleSubmit =(event) =>{

        let user={id:this.state.username,
                name:this.state.name,
                passwd:this.state.passwd
        }
        console.log('A user was submitted: ');
        console.log(user);
        this.props.addFunc(user);
        event.preventDefault();
    }

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <label>
                    Username:
                    <input type="text" value={this.state.username} onChange={this.handleUserChange} />
                </label><br/>
                <label>
                    Name:
                    <input type="text" value={this.state.name} onChange={this.handleNameChange} />
                </label><br/>
                <label>
                    Passwd:
                    <input type="password" value={this.state.passwd} onChange={this.handlePasswdChange} />
                </label><br/>

                <input type="submit" value="Add user" />
            </form>
        );
    }
}
export default UserForm;*/