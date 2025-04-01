
import React, {useEffect} from 'react';
import { useState } from 'react';
import UserTable from './UserTable.jsx';
import './UserApp.css'
import UserForm from "./UserForm.jsx";
import {GetUsers, DeleteUser, AddUser} from './utils/rest-calls'


export default function UserApp(){
	const [users, setUsers] = useState([{"passwd":"maria","name":"Marinescu Maria","id":"maria"}]);

	function addFunc(user){
		console.log('inside add Func '+user);
		AddUser(user)
			.then(res=> GetUsers())
			.then(users=>setUsers(users))
			.catch(erorr=>console.log('eroare add ',erorr));
	}

	function deleteFunc(user){
		console.log('inside deleteFunc '+user);
		DeleteUser(user)
			.then(res=> GetUsers())
			.then(users=>setUsers(users))
			.catch(error=>console.log('eroare delete', error));
	}
	useEffect(()=>{
		console.log('inside useEffect')
		GetUsers().then(users=>setUsers(users));},[]);

	return (<div className="UserApp">
		<h1> New User Management App </h1>
		<UserForm addFunc={addFunc}/>
		<br/>
		<br/>
		<UserTable users={users} deleteFunc={deleteFunc}/>
	</div>);
}
/*
class UserApp extends React.Component{
    constructor(props){
        super(props);
             this.state={users:[{"passwd":"maria","name":"Marinescu Maria","id":"maria"}]}
        //deleteFunc:this.deleteFunc.bind(this);
         //addFunc:this.addFunc.bind(this);
        console.log('UserApp constructor')
    }

    addFunc=(user)=>{
        console.log('inside add Func '+user);
        AddUser(user)
            .then(res=> GetUsers())
            .then(users=>this.setState({users}))
            .catch(erorr=>console.log('eroare add ',erorr));
    }


    deleteFunc=(user)=>{
        console.log('inside deleteFunc '+user);
        DeleteUser(user)
            .then(res=> GetUsers())
            .then(users=>this.setState({users}))
            .catch(error=>console.log('eroare delete', error));
    }


    componentDidMount(){
        console.log('inside componentDidMount')
        GetUsers().then(users=>this.setState({users}));
    }

    render(){
        return(
            <div className="UserApp">
                <h1>Chat User Management</h1>
                <UserForm addFunc={this.addFunc}/>
                <br/>
                <br/>
                 <UserTable users={this.state.users} deleteFunc={this.deleteFunc}/>
            </div>
        );
    }
}

export default UserApp;*/
