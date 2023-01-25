function Input({onChange, name, value, type, label, readonly}) {
    return (
        <div className="required form-group">
            <label>{label}</label>
            <input className="form-control text-box single-line" type={type} value={value} onChange={onChange} name={name} readOnly={readonly}/>
        </div>
    )
}
export default Input;