import axios from "axios";
import { useEffect, useState } from "react";

import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function ConvertRCG() {

const [inputText, setInputText] = useState("");
const [outputText, setOutputText] = useState("");
const [isProcessing, setIsProcessing] = useState(false);
const [showComponent, setShowComponent] = useState(null);
const [showComponent2, setShowComponent2] = useState(null);

  
  useEffect(() => {
    const interval = setInterval(() => {
        Load()
    }, 1000);

    return () => clearInterval(interval);
  }, []);

  async function Load() {
    const result = await axios.get("http://localhost:5171/api/Convert/GetEncodedStrings");
    setOutputText(result.data);
  }

  async function notifySuccess(message) {
    toast.success(message);
  }
  async function notifyInfo(message) {
    toast.error(message);
  }

  async function Cancel() {
    setShowComponent2("show");
    const result = await axios.get("http://localhost:5171/api/Convert/CancelEncode");
    notifyInfo();
  }

  async function encode(event) {
    if(!isProcessing){
setShowComponent("show");
        setIsProcessing(true);
    event.preventDefault();
    try {
        let response = await axios.post("http://localhost:5171/api/Convert/Converter", inputText, {
          headers: {
            "Content-Type": "application/json",
          },
        });

        debugger;
        
        const res = response.data;
        if(res.isSuccess){
          notifySuccess(res.message)
          setShowComponent2(null);
        }
        else{
          notifyInfo(res.message)
          setIsProcessing(false);
        }
      } catch (error) {
        alert(error)
      } 
      finally{
        setIsProcessing(false);
        setShowComponent(null);
      }
    }
  }

    return (
      <div>
        <div className="split left">
            <div className="centered">
            <h1 className="mb-5">Encode String</h1>
            <input
              type="text"
              className="form-control encodeTxtBx"
              id="EncodeStr"
              onChange={(event) => {
                setInputText(event.target.value);
              }}
            />
             <button className="btn btn-outline-primary mt-4 encodeBtn" onClick={encode} disabled={isProcessing}>
                Convert
                </button><br/>
            <div hide style={showComponent ? {} : { display: 'none' }} class="spinner-border text-primary m-5" role="status">
            <span class="sr-only"></span>
          </div>
            </div>


        </div>

        <div className="split right">
            <ToastContainer />
            <div className="centered">
                <h1 className="mb-5">Output Text</h1>
                <input
                type="text"
                className="form-control encodeTxtBx"
                id="outputText"
                value={outputText}
                readOnly
                />
                <button className="btn btn-outline-danger mt-4 encodeBtn" disabled={!isProcessing} onClick={Cancel}>
              Cancel
            </button>
            <br/>
            <div hide style={showComponent2 ? {} : { display: 'none' }} class="spinner-border text-danger m-5" role="status">
            <span class="sr-only"></span>
          </div>
            </div>
        </div>
      </div>
    );
  }
  
  export default ConvertRCG;
  