import * as constants from '../constants';

// js library for handling cookies sfcookies install cmd <npm install sfcookies --save>
import { bake_cookie, read_cookie } from 'sfcookies';

function reminder (action){
    let {text, dueDate } = action;
    return{
        text,
        dueDate,
        id: Math.random()
    }
};

function removeById(state=[], id){
    const reminders = state.filter(r => r.id !== id);
    console.log('new reduced reminders', reminders);
    return reminders;
}

const reminders = (state = [], action) => {
    let reminders = null;
    state = read_cookie('reminders');
    console.log(constants.ADD_REMINDER);
    switch(action.type){
        case constants.ADD_REMINDER:
            reminders = [...state, reminder(action)];
            bake_cookie('reminders', reminders)
            return reminders;
        case constants.DELETE_REMINDER:
            reminders = removeById(state, action.id);
            bake_cookie('reminders', reminders);
            return reminders;
        case constants.CLEAR_REMINDERS:
            reminders = [];
            bake_cookie('reminders', reminders);
            return reminders;
        default:
            return state;
    }
};

export default reminders;