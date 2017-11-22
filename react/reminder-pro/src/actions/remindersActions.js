import * as constants from '../constants';

export function addReminder(text, dueDate){
    return {
        type: constants.ADD_REMINDER,
        text,
        dueDate
    };
}

export const deleteReminder = (id) => {
    const action = {
        type: constants.DELETE_REMINDER,
        id
    }
    return action;
}

export function clearReminders(){
    return {
        type: constants.CLEAR_REMINDERS,
    };
}