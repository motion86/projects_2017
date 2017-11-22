import React, {Component} from 'react';
import Clock from './Clock';
import './app.css';
import { Form, FormControl, Button } from 'react-bootstrap';

class App extends Component{
    constructor(props){
        super(props);
        this.state = {
            deadline: 'December 25, 2017'
        }
    }

    changeDeadline(){
        console.log('state', this.state);
        this.setState({deadline: this.state.newDeadline})
    }

    render(){
        return(
            <div className="App">
                <div className='app-title'>Countdown to {this.state.deadline}</div>
                <Clock 
                    deadline={this.state.deadline}
                />
                <Form inline>
                    <FormControl
                        className='deadline-input' 
                        placeholder="new date" 
                        onChange={event => this.setState({newDeadline: event.target.value})}
                    />
                    <Button onClick={()=>this.changeDeadline()}>
                        Submit
                    </Button>
                </Form>
            </div>
        );
    }
}

export default App;