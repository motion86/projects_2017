import React, { Component } from 'react';
import '../app.css';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { addReminder, deleteReminder, clearReminders } from '../actions/remindersActions';

// to install moment run <nmp install moment --save> momnet formats js dates.
import moment from 'moment';

class App extends Component {
    constructor(props){
        super(props);
        this.state ={
            text: '',
            dueDate: ''
        };
    }

    addReminder(){
        this.props.addReminder(this.state.text, this.state.dueDate);
    }

    renderReminders(){
        const  {reminders} = this.props;
        return (
            <ul className='list-group col-sm-4'>
                {
                    reminders.map(r => {
                        return(
                            <li key={r.id} className='list-group-item'>
                                <div>
                                    <div className='list-item'>{r.text}</div>
                                    <div className='list-item'><em>{moment(new Date(r.dueDate)).fromNow()}</em></div>
                                </div>
                                <div className='list-item delete-button' onClick={() => this.deleteReminder(r.id)}>&#x2715;</div>
                            </li>
                        );
                    })
                }
            </ul>
        );
    }

    deleteReminder(id){
        this.props.deleteReminder(id);
    }

    render() {
        
        return (
            <div className='app'>
                <div className='title'>
                    My Reminder
                </div>
                <div className='form-inline reminder-form'>
                    <div className='form-group'>
                        <input 
                            className='form-control' 
                            placeholder='I have to..'
                            onChange={e => this.setState({text: e.target.value})}
                        />
                        <input 
                            className='form-control' 
                            type='datetime-local'
                            onChange={e => this.setState({dueDate: e.target.value})}
                        />
                    </div>
                    <button 
                        className='btn btn-success'
                        onClick={() => this.addReminder()}
                    >
                        Add Reminder
                    </button>
                </div>
                {this.renderReminders()}
                <div className='btn btn-danger' onClick={() => this.props.clearReminders()}>
                    Clear All Reminders
                </div>
            </div>
        );
    }
}

function mapStateToProps(state){
    console.log('state', state);
    return {
        reminders: state
    }
}

// function mapDispatchToProps(dispatch){
//     return bindActionCreators({ addReminder }, dispatch);
// }

// function mapDispatchToProps(dispatch){
//     return{
//         
//         setName: (somePayload) => {
//             dispatch({
//                 type: 'SOME_TYPE',
//                 payload: somePayload
//             });
//         }
//     }
// }

// This defines which properties of the global app state u want to use here and to which properties u wanna map them. key represents local prop and value is the global state value we wanna use. eg. state.myReducer.myProp
// const mapStateToProps = (state) => {
//     return {
//         prop1: state.myReducer1.myProp1,
//         prop2: state.myReducer2.myProp2
//     }
// }


// connect tells redux that you wanna connect this component to the redux store.
//export default connect(null, mapDispatchToProps)(App);

// this is a shortcut and we dont need dispatchToProps this way
export default connect(mapStateToProps, { addReminder, deleteReminder, clearReminders })(App);