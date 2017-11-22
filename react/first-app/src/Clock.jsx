import React, { Component } from 'react';
import './app.css';

class Clock extends Component {
    constructor(props){
        super(props);
        this.state = {
            days:0,
            hours:0,
            minutes:0,
            seconds:0
        }
    }

    componentWillMount(){
        this.getTimeUntil(this.props.deadline);
    }

    componentDidMount(){
        setInterval(() => this.getTimeUntil(this.props.deadline), 1000);
    }

    getTimeUntil(deadline){
        const time = Date.parse(deadline) - Date.parse(new Date());
        console.log('time', time);
        const s = time/1000;
        const seconds = Math.floor(s%60);
        const minutes = Math.floor((s/60)%60);
        const hours = Math.floor((s/60/60)%24);
        const days = Math.floor((s/60/60/24));

        this.setState({ seconds,minutes,hours,days });
    }

    leadingZero(num){
        return num < 10 ? '0' + num : num;
    }

    render() {
        return (
            <div>
                <div className='my-clock'>{this.leadingZero(this.state.days)} days</div>
                <div className='my-clock'>{this.leadingZero(this.state.hours)} hours</div>
                <div className='my-clock'>{this.leadingZero(this.state.minutes)} minutes</div>
                <div className='my-clock'>{this.leadingZero(this.state.seconds)} seconds</div>
            </div>
        );
    }
}

export default Clock;