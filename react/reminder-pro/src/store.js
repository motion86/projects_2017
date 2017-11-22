import {createStore, combineReducers, applyMiddleware} from 'redux';
import reducer from './reducers/remindersReducer';

// a good third party logger can be added.. install with npm install redux-logger --save
import logger from 'redux-logger'

// const myLogger = (store) => (next) => (action) => {
//     console.log('Logged Action: ', action)
//     next(action); // this method MUST be called in order for execution to continue.
// }

export default createStore(reducer); // if more than one reducer use createStore(combineReducers({reducer1, reducer2, etc..}))
                                    // if using middleware initialize store like this .. createStore(reducer, initialState, applyMiddleware(myLogger)) u can pass multiple middleware separating them by comma