import os
import plantuml

def generate_url(file_path, output_file):
    with open(file_path, 'r') as file:
        diagram = file.read()

    pl = plantuml.PlantUML(url='https://www.plantuml.com/plantuml/svg/')
    url = pl.get_url(diagram)

    markdown_content = f"## Diagram\n\n[Link to diagram]({url})"

    with open(output_file, 'w') as f:
        f.write(markdown_content)

if __name__ == "__main__":
    file_path = os.getenv('FILE_PATH', 'include.puml')
    output_file = 'url.txt'
    generate_url(file_path, output_file)
