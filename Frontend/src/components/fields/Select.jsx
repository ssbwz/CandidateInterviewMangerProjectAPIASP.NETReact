function Select({label, name, value, onChange, options}) {
    return (
        <div className="required form-group">
            <label>{label}</label>
            <select className="custom-select" value={value} name={name} onChange={onChange} >
                <option value="" disabled selected>...</option>
                {
                    options.map((option, index) => {
                        return <option key={index} value={option.value}>{option.label}</option>
                    })
                }
            </select>
        </div>
    )
}

export default Select;